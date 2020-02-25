module PlayingCards


/// https://fable.io/



module Cards1 =
    type Face = Ace | King | Queen | Jack | Number of int
    type Color = Spades | Hearts | Diamonds | Clubs
    type Card = Face * Color

    let aceOfHearts = Ace,Hearts
    let tenOfSpades = (Number 10), Spades

    let showMe card =
        match card with     // generates warning: Incomplete pattern matches on this expression
        | Ace, Hearts -> printfn "Ace of Hearts!"
        | _, Hearts -> printfn "other heart"
        | (Number 10), Spades -> printfn "10 of Spades"
        | _, (Diamonds|Clubs) -> printfn "Diamonds or Clubs"

    printfn "============================================================================"


module Cards2 =
    /// Suit of a playing card
    type Suit =
        | Hearts
        | Clubs
        | Diamonds
        | Spades
 
    /// A Discriminated Union can also be used to represent the rank of a playing card
    type Rank =
        | Value of int
        | Ace
        | King
        | Queen
        | Jack
     
        static member GetAllRanks() = [
            yield Ace
            for i in 2..10 do yield Value i
            yield Jack
            yield Queen
            yield King ]
          
    /// Record type that combines a Suit and a Rank
    type Card = { Suit: Suit; Rank: Rank }

    /// Compute a list representing all cards in the deck
    let fullDeck =
        [ for suit in [ Hearts; Diamonds; Clubs; Spades ] do
            for rank in Rank.GetAllRanks() do
                yield { Suit=suit; Rank=rank } ]

    let showPlayingCard (c:Card) =
        let rankString =
            match c.Rank with
            | Ace -> "Ace"
            | King -> "King"
            | Queen -> "Queen"
            | Jack -> "Jack"
            | Value n -> string n
        let suitString =
            match c.Suit with
            | Clubs -> "clubs"
            | Diamonds -> "diamonds"
            | Spades -> "spades"
            | Hearts -> "hearts"
        rankString + " of " + suitString

    /// Print all the playing cards in a deck
    let printAllCards() =
        for card in fullDeck do
            printfn "%s" (showPlayingCard card)

        







