using static Zugfish.Engine.Translation;

namespace Zugfish.Engine;

public class Board
{
    #region fields
    private bool IsWhiteTurn { get; set; }
    private Bitboard _whitePawns;
    private Bitboard _whiteKnights;
    private Bitboard _whiteBishops;
    private Bitboard _whiteRooks;
    private Bitboard _whiteQueens;
    private Bitboard _whiteKing;

    private Bitboard _blackPawns;
    private Bitboard _blackKnights;
    private Bitboard _blackBishops;
    private Bitboard _blackRooks;
    private Bitboard _blackQueens;
    private Bitboard _blackKing;

    public Bitboard WhitePawns { get => _whitePawns; private set => _whitePawns = value; }
    public Bitboard WhiteKnights { get => _whiteKnights; private set => _whiteKnights = value; }
    public Bitboard WhiteBishops { get => _whiteBishops; private set => _whiteBishops = value; }
    public Bitboard WhiteRooks { get => _whiteRooks; private set => _whiteRooks = value; }
    public Bitboard WhiteQueens { get => _whiteQueens; private set => _whiteQueens = value; }
    public Bitboard WhiteKing { get => _whiteKing; private set => _whiteKing = value; }

    public Bitboard BlackPawns { get => _blackPawns; private set => _blackPawns = value; }
    public Bitboard BlackKnights { get => _blackKnights; private set => _blackKnights = value; }
    public Bitboard BlackBishops { get => _blackBishops; private set => _blackBishops = value; }
    public Bitboard BlackRooks { get => _blackRooks; private set => _blackRooks = value; }
    public Bitboard BlackQueens { get => _blackQueens; private set => _blackQueens = value; }
    public Bitboard BlackKing { get => _blackKing; private set => _blackKing = value; }

    public Bitboard WhitePieces { get; private set; }
    public Bitboard BlackPieces { get; private set; }
    public Bitboard AllPieces { get; private set; }

    private readonly Stack<MoveUndo> _moveHistory = new(); // TODO: initialize stack with capacity matching max depth
    public ushort CastlingRights { get; private set; }
    public int EnPassantTarget { get; private set; } = -1;
    #endregion

    public Board()
    {
        WhitePawns = new Bitboard(0xFF00);
        WhiteKnights = new Bitboard(0x42);
        WhiteBishops = new Bitboard(0x24);
        WhiteRooks = new Bitboard(0x81);
        WhiteQueens = new Bitboard(0x8);
        WhiteKing = new Bitboard(0x10);
        BlackPawns = new Bitboard(0xFF000000000000);
        BlackKnights = new Bitboard(0x4200000000000000);
        BlackBishops = new Bitboard(0x2400000000000000);
        BlackRooks = new Bitboard(0x8100000000000000);
        BlackQueens = new Bitboard(0x800000000000000);
        BlackKing = new Bitboard(0x1000000000000000);
        CastlingRights = 0b1111;
        IsWhiteTurn = true;
        DeriveCombinedBitboards();
    }

    public Board(ReadOnlySpan<char> fen)
    {
        var enumerator = fen.Split(' ');
        enumerator.MoveNext();

        // Parse piece placement
        var piecePlacement = fen[enumerator.Current]; enumerator.MoveNext();
        var square = 56; // Start at a8
        foreach (var c in piecePlacement)
        {
            if (char.IsDigit(c))
                square += c - '0'; // Empty squares
            else if (c == '/')
                square -= 16; // Move to next rank
            else
            {
                Bitboard pieceMask = 1UL << square++;
                switch (c)
                {
                    case 'P':
                        WhitePawns |= pieceMask;
                        break;
                    case 'N':
                        WhiteKnights |= pieceMask;
                        break;
                    case 'B':
                        WhiteBishops |= pieceMask;
                        break;
                    case 'R':
                        WhiteRooks |= pieceMask;
                        break;
                    case 'Q':
                        WhiteQueens |= pieceMask;
                        break;
                    case 'K':
                        WhiteKing |= pieceMask;
                        break;
                    case 'p':
                        BlackPawns |= pieceMask;
                        break;
                    case 'n':
                        BlackKnights |= pieceMask;
                        break;
                    case 'b':
                        BlackBishops |= pieceMask;
                        break;
                    case 'r':
                        BlackRooks |= pieceMask;
                        break;
                    case 'q':
                        BlackQueens |= pieceMask;
                        break;
                    case 'k':
                        BlackKing |= pieceMask;
                        break;
                    default:
                        throw new ArgumentException($"Invalid FEN piece: {c}");
                }
            }
        }

        var activeColor = fen[enumerator.Current]; enumerator.MoveNext();
        IsWhiteTurn = activeColor[0] switch
        {
            'w' => true,
            'b' => false,
            _ => throw new ArgumentException("Invalid active color.")
        };

        // Parse castling rights
        var castlingRights = fen[enumerator.Current]; enumerator.MoveNext();

        if (castlingRights.IndexOf('K') != -1) CastlingRights |= 0b0001;
        if (castlingRights.IndexOf('Q') != -1) CastlingRights |= 0b0010;
        if (castlingRights.IndexOf('k') != -1) CastlingRights |= 0b0100;
        if (castlingRights.IndexOf('q') != -1) CastlingRights |= 0b1000;

        // Parse en passant square
        var enPassantSquare = fen[enumerator.Current]; enumerator.MoveNext();
        if (enPassantSquare is not "-")
        {
            var file = enPassantSquare[0];
            var rank = enPassantSquare[1];
            EnPassantTarget = 8 * (rank - '1') + (file - 'a');
        }
        else
        {
            EnPassantTarget = -1;
        }

        var halfmoveClock = fen[enumerator.Current]; enumerator.MoveNext();

        var fullmoveNumber = fen[enumerator.Current]; enumerator.MoveNext();

        // Update combined bitboards
        DeriveCombinedBitboards();
    }

    public void MakeMove(ReadOnlySpan<char> uciMove)
    {
        if (uciMove.Length is < 4 or > 5)
            throw new ArgumentException("Invalid UCI move format.", nameof(uciMove));

        var from = SquareFromUci(uciMove[0], uciMove[1]);
        var to = SquareFromUci(uciMove[2], uciMove[3]);

        if ((from | to) >> 6 != 0) // Ensure indices are valid
            throw new ArgumentOutOfRangeException(nameof(uciMove), "Square index out of range.");

        // Default to a quiet move
        var moveType = MoveType.Quiet;

        var isPawnMove = (WhitePawns & (1UL << from)) != 0 || (BlackPawns & (1UL << from)) != 0;

        // Detect castling (if king moves two squares)
        if ((from == 4 && to is 2 or 6) || (from == 60 && to is 58 or 62))
        {
            moveType = MoveType.Castling;
        }
        else if (uciMove.Length == 5) // If the move has a 5th character (promotion), determine its type
        {
            moveType = uciMove[4] switch
            {
                'q' => MoveType.PromoteToQueen,
                'r' => MoveType.PromoteToRook,
                'b' => MoveType.PromoteToBishop,
                'n' => MoveType.PromoteToKnight,
                _ => throw new ArgumentException("Invalid promotion piece.", nameof(uciMove))
            };
        }
        else if (isPawnMove && EnPassantTarget == to) // Detect En Passant
        {
            moveType = MoveType.EnPassant;
        }
        else if (isPawnMove && Math.Abs(from - to) == 16) // Detect double pawn move
        {
            moveType = MoveType.DoublePawnPush;
        }

        var move = new Move(from, to, moveType);
        MakeMove(move);
    }

    public void MakeMove(Move move)
    {
        var from = move.From;
        var to = move.To;
        Bitboard fromMask = 1UL << from;
        Bitboard toMask = 1UL << to;

        // Determine captured piece
        var capturedPieceMask = DetermineCapturedPiece(move.Type, from, to, toMask);

        // Save undo information
        SaveMoveUndo(move, fromMask, toMask, capturedPieceMask);

        // Get the moving piece's bitboard
        ref var movingPieceBoard = ref GetPieceBitboard(fromMask);

        switch (move.Type)
        {
            case MoveType.Quiet:
            case MoveType.Capture:
                HandleQuietOrCaptureMove(ref movingPieceBoard, toMask);
                break;
            case MoveType.Castling:
                HandleCastlingMove(move, ref movingPieceBoard, toMask);
                break;
            case MoveType.DoublePawnPush:
                HandleDoublePawnPushMove(ref movingPieceBoard, toMask);
                break;
            case MoveType.EnPassant:
                HandleEnPassantMove(move, from, to, toMask);
                break;
            case MoveType.PromoteToKnight:
            case MoveType.PromoteToBishop:
            case MoveType.PromoteToRook:
            case MoveType.PromoteToQueen:
                HandlePromotionMove(move, to, toMask);
                break;
            default:
                throw new InvalidOperationException("Unhandled move type");
        }

        // Remove the moving piece from its original square
        movingPieceBoard &= ~fromMask;

        // Remove any captured piece and update castling rights if necessary
        if (capturedPieceMask != 0)
        {
            HandleCapturedPiece(capturedPieceMask);
        }

        UpdateCastlingRightsAfterMove(from);

        EnPassantTarget = move.Type == MoveType.DoublePawnPush ? (from + to) / 2 : -1;

        // Recalculate combined bitboards
        DeriveCombinedBitboards();
        IsWhiteTurn = !IsWhiteTurn;
    }

    private Bitboard DetermineCapturedPiece(MoveType moveType, int from, int to, Bitboard toMask)
    {
        var captured = AllPieces & toMask;
        if (moveType != MoveType.EnPassant)
        {
            return captured;
        }

        // En Passant capture
        var capturedPawnSquare = to + (to > from ? -8 : 8);
        captured = 1UL << capturedPawnSquare;
        return captured;
    }

    private void SaveMoveUndo(Move move, Bitboard fromMask, Bitboard toMask, Bitboard capturedPieceMask)
    {
        var capturedType = PieceType.None;
        if (capturedPieceMask != 0)
        {
            capturedType = GetPieceTypeWithOverlap(capturedPieceMask);
        }

        _moveHistory.Push(new MoveUndo
        {
            CapturedPiece = capturedPieceMask,
            FromSquare = fromMask,
            ToSquare = toMask,
            Move = move,
            PreviousCastlingRights = CastlingRights,
            PreviousEnPassantTarget = EnPassantTarget,
            CapturedPieceType = capturedType
        });
    }

    private void HandleQuietOrCaptureMove(ref Bitboard pieceBoard, Bitboard toMask)
    {
        pieceBoard |= toMask;
    }

    private void HandleCastlingMove(Move move, ref Bitboard pieceBoard, Bitboard toMask)
    {
        // Move the king
        pieceBoard |= toMask;

        // Move the rook based on destination square
        switch (move.To)
        {
            case 2: // White Queen-side castling
                WhiteRooks &= ~(1UL << 0); // Remove rook from a1
                WhiteRooks |= 1UL << 3;  // Place rook on d1
                break;
            case 6: // White King-side castling
                WhiteRooks &= ~(1UL << 7); // Remove rook from h1
                WhiteRooks |= 1UL << 5;  // Place rook on f1
                break;
            case 58: // Black Queen-side castling
                BlackRooks &= ~(1UL << 56); // Remove rook from a8
                BlackRooks |= 1UL << 59;  // Place rook on d8
                break;
            case 62: // Black King-side castling
                BlackRooks &= ~(1UL << 63); // Remove rook from h8
                BlackRooks |= 1UL << 61;  // Place rook on f8
                break;
        }
    }

    private void HandleDoublePawnPushMove(ref Bitboard pieceBoard, Bitboard toMask)
    {
        pieceBoard |= toMask;
    }

    private void HandleEnPassantMove(Move move, int from, int to, Bitboard toMask)
    {
        ref var pawnBoard = ref GetPieceBitboard(1UL << from);
        pawnBoard |= toMask;
        // Removal of the captured pawn is handled by the captured piece logic.
    }

    private void HandlePromotionMove(Move move, int to, Bitboard toMask)
    {
        bool isWhite = to > 55; // Adjust based on your board representation
        switch (move.Type)
        {
            case MoveType.PromoteToKnight:
                if (isWhite) WhiteKnights |= toMask; else BlackKnights |= toMask;
                break;
            case MoveType.PromoteToBishop:
                if (isWhite) WhiteBishops |= toMask; else BlackBishops |= toMask;
                break;
            case MoveType.PromoteToRook:
                if (isWhite) WhiteRooks |= toMask; else BlackRooks |= toMask;
                break;
            case MoveType.PromoteToQueen:
                if (isWhite) WhiteQueens |= toMask; else BlackQueens |= toMask;
                break;
            default:
                throw new InvalidOperationException("Invalid promotion move type");
        }
    }

    private void HandleCapturedPiece(Bitboard capturedPieceMask)
    {
        ref var capturedBoard = ref GetPieceBitboard(capturedPieceMask);
        capturedBoard &= ~capturedPieceMask;

        // Update castling rights if a rook was captured
        if ((capturedPieceMask & WhiteRooks) != 0)
        {
            if ((capturedPieceMask & (1UL << 0)) != 0) CastlingRights &= 0b1101;
            if ((capturedPieceMask & (1UL << 7)) != 0) CastlingRights &= 0b1110;
        }
        else if ((capturedPieceMask & BlackRooks) != 0)
        {
            if ((capturedPieceMask & (1UL << 56)) != 0) CastlingRights &= 0b0111;
            if ((capturedPieceMask & (1UL << 63)) != 0) CastlingRights &= 0b1011;
        }
    }

    private void UpdateCastlingRightsAfterMove(int from)
    {
        switch (from)
        {
            case 4:
                CastlingRights &= 0b1100;
                break;
            case 60:
                CastlingRights &= 0b0011;
                break;
            case 0:
                CastlingRights &= 0b1101;
                break;
            case 7:
                CastlingRights &= 0b1110;
                break;
            case 56:
                CastlingRights &= 0b0111;
                break;
            case 63:
                CastlingRights &= 0b1011;
                break;
        }
    }

    public void UnmakeMove()
    {
        if (_moveHistory.Count == 0)
            throw new InvalidOperationException("No move to unmake.");

        var lastUndo = _moveHistory.Pop();
        var move = lastUndo.Move;

        // Dispatch undo handling based on move type
        switch (move.Type)
        {
            case MoveType.Quiet:
            case MoveType.Capture:
                UndoQuietOrCaptureMove(lastUndo);
                break;
            case MoveType.Castling:
                UndoCastlingMove(lastUndo);
                break;
            case MoveType.DoublePawnPush:
                UndoDoublePawnPushMove(lastUndo);
                break;
            case MoveType.EnPassant:
                UndoEnPassantMove(lastUndo);
                break;
            case MoveType.PromoteToKnight:
            case MoveType.PromoteToBishop:
            case MoveType.PromoteToRook:
            case MoveType.PromoteToQueen:
                UndoPromotionMove(lastUndo);
                break;
            default:
                throw new InvalidOperationException("Unhandled move type during undo");
        }

        // Restore captured piece if any
        if (lastUndo.CapturedPiece != 0)
        {
            ref var capturedBoard = ref GetPieceBitboard(lastUndo.CapturedPieceType);
            capturedBoard |= lastUndo.CapturedPiece;
        }

        EnPassantTarget = lastUndo.PreviousEnPassantTarget;
        CastlingRights = lastUndo.PreviousCastlingRights;

        DeriveCombinedBitboards();
        IsWhiteTurn = !IsWhiteTurn;
    }

    private void UndoQuietOrCaptureMove(MoveUndo lastUndo)
    {
        ref var movedPieceBoard = ref GetPieceBitboard(lastUndo.ToSquare);
        movedPieceBoard &= ~lastUndo.ToSquare;
        movedPieceBoard |= lastUndo.FromSquare;
    }

    private void UndoCastlingMove(MoveUndo lastUndo)
    {
        ref var kingBoard = ref GetPieceBitboard(lastUndo.ToSquare);
        kingBoard &= ~lastUndo.ToSquare;
        kingBoard |= lastUndo.FromSquare;

        switch (lastUndo.Move.To)
        {
            case 2:
                WhiteRooks &= ~(1UL << 3);
                WhiteRooks |= 1UL << 0;
                break;
            case 6:
                WhiteRooks &= ~(1UL << 5);
                WhiteRooks |= 1UL << 7;
                break;
            case 58:
                BlackRooks &= ~(1UL << 59);
                BlackRooks |= 1UL << 56;
                break;
            case 62:
                BlackRooks &= ~(1UL << 61);
                BlackRooks |= 1UL << 63;
                break;
        }
    }

    private void UndoDoublePawnPushMove(MoveUndo lastUndo)
    {
        UndoQuietOrCaptureMove(lastUndo);
    }

    private void UndoEnPassantMove(MoveUndo lastUndo)
    {
        // First, undo the moving pawn's relocation.
        UndoQuietOrCaptureMove(lastUndo);

        // Calculate the square of the captured pawn.
        var capturedPawnSquare = lastUndo.Move.To + (lastUndo.Move.To > lastUndo.Move.From ? -8 : 8);
        Bitboard capturedPawnMask = 1UL << capturedPawnSquare;

        // Use the captured pawn type from the undo record.
        ref var pawnBoard = ref GetPieceBitboard(lastUndo.CapturedPieceType);
        pawnBoard |= capturedPawnMask;
    }

    private void UndoPromotionMove(MoveUndo lastUndo)
    {
        var isWhite = lastUndo.Move.To > 55;
        if (isWhite)
        {
            ref var pawnBoard = ref GetPieceBitboard(PieceType.WhitePawn);
            pawnBoard |= lastUndo.FromSquare;
        }
        else
        {
            ref var pawnBoard = ref GetPieceBitboard(PieceType.BlackPawn);
            pawnBoard |= lastUndo.FromSquare;
        }

        switch (lastUndo.Move.Type)
        {
            case MoveType.PromoteToKnight:
                if (isWhite) WhiteKnights &= ~lastUndo.ToSquare; else BlackKnights &= ~lastUndo.ToSquare;
                break;
            case MoveType.PromoteToBishop:
                if (isWhite) WhiteBishops &= ~lastUndo.ToSquare; else BlackBishops &= ~lastUndo.ToSquare;
                break;
            case MoveType.PromoteToRook:
                if (isWhite) WhiteRooks &= ~lastUndo.ToSquare; else BlackRooks &= ~lastUndo.ToSquare;
                break;
            case MoveType.PromoteToQueen:
                if (isWhite) WhiteQueens &= ~lastUndo.ToSquare; else BlackQueens &= ~lastUndo.ToSquare;
                break;
        }
    }

    private PieceType GetPieceTypeWithOverlap(Bitboard capturedPieceMask)
    {
        if ((WhitePawns &  capturedPieceMask) != 0) return PieceType.WhitePawn;
        if ((WhiteKnights & capturedPieceMask) != 0) return PieceType.WhiteKnight;
        if ((WhiteBishops & capturedPieceMask) != 0) return PieceType.WhiteBishop;
        if ((WhiteRooks  & capturedPieceMask) != 0) return PieceType.WhiteRook;
        if ((WhiteQueens & capturedPieceMask) != 0) return PieceType.WhiteQueen;
        if ((BlackPawns &  capturedPieceMask) != 0) return PieceType.BlackPawn;
        if ((BlackKnights & capturedPieceMask) != 0) return PieceType.BlackKnight;
        if ((BlackBishops & capturedPieceMask) != 0) return PieceType.BlackBishop;
        if ((BlackRooks  & capturedPieceMask) != 0) return PieceType.BlackRook;
        return PieceType.BlackQueen;
    }

    private ref Bitboard GetPieceBitboard(Bitboard bitboard)
    {
        if ((WhitePawns & bitboard) != 0) return ref _whitePawns;
        if ((WhiteKnights & bitboard) != 0) return ref _whiteKnights;
        if ((WhiteBishops & bitboard) != 0) return ref _whiteBishops;
        if ((WhiteRooks & bitboard) != 0) return ref _whiteRooks;
        if ((WhiteQueens & bitboard) != 0) return ref _whiteQueens;
        if ((WhiteKing & bitboard) != 0) return ref _whiteKing;

        if ((BlackPawns & bitboard) != 0) return ref _blackPawns;
        if ((BlackKnights & bitboard) != 0) return ref _blackKnights;
        if ((BlackBishops & bitboard) != 0) return ref _blackBishops;
        if ((BlackRooks & bitboard) != 0) return ref _blackRooks;
        if ((BlackQueens & bitboard) != 0) return ref _blackQueens;
        if ((BlackKing & bitboard) != 0) return ref _blackKing;

        throw new InvalidOperationException("No matching piece found for given bitboard.");
    }

    private ref Bitboard GetPieceBitboard(PieceType pieceType)
    {
        switch (pieceType)
        {
            case PieceType.WhitePawn: return ref _whitePawns;
            case PieceType.WhiteKnight: return ref _whiteKnights;
            case PieceType.WhiteBishop: return ref _whiteBishops;
            case PieceType.WhiteRook: return ref _whiteRooks;
            case PieceType.WhiteQueen: return ref _whiteQueens;
            case PieceType.BlackPawn: return ref _blackPawns;
            case PieceType.BlackKnight: return ref _blackKnights;
            case PieceType.BlackBishop: return ref _blackBishops;
            case PieceType.BlackRook: return ref _blackRooks;
            case PieceType.BlackQueen: return ref _blackQueens;
            case PieceType.None:
            default: throw new InvalidOperationException("No matching piece found for given piece type.");
        }
    }

    private void DeriveCombinedBitboards()
    {
        WhitePieces = WhitePawns | WhiteKnights | WhiteBishops | WhiteRooks | WhiteQueens | WhiteKing;
        BlackPieces = BlackPawns | BlackKnights | BlackBishops | BlackRooks | BlackQueens | BlackKing;
        AllPieces = WhitePieces | BlackPieces;
    }
}
