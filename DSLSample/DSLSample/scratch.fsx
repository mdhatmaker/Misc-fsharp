type CardType =
    | Visa
    | Mastercard
type CheckNumber = CheckNumber of int
type CardNumber = CardNumber of string

type CreditCardInfo =
    { CardType: CardType
      CardNumber: CardNumber }

type PaymentMethod =
    | Cash
    | Check of CheckNumber
    | Card of CreditCardInfo

type PaymentAmount = PaymentAmount of decimal
type Currency = EUR | USD

type Payment =
    { Amount: PaymentAmount
      Currency: Currency
      Method: PaymentMethod }

///////////////////////////////////////////////////////////

type [<Measure>] g
type [<Measure>] inch
type Grams = Grams of int<g>
type Inches = Inches of float<inch>

type GadgetName = GadgetName of string

type PhoneCode = PhoneCode of string    // 5 characters starting with T
type TabletCode = TabletCode of int     // 10000 < code < 100000

type GadgetCode =
    | PhoneCode of PhoneCode
    | TabletCode of TabletCode

type Gadget =
    { Code: GadgetCode
      Name: GadgetName
      Weight: Grams
      ScreenSize: Inches }

///////////////////////////////////////////////////////////

type Result<'Success,'Failure> =
    | Ok of 'Success
    | Error of 'Failure

type PaymentError =
    | CardTypeNotRecognized
    | PaymentRejected
    | PaymentProviderOffline

//type PayInvoice = UnpaidInvoice -> Payment -> Result<PaidInvoice,PaymentError>

///////////////////////////////////////////////////////////

let add arg1 arg2 =
    arg1 + arg2

let result = add 100 200

//let addFloats (arg1:float) (arg2:float) : float =
//    arg1 + arg2

type AddFloats = float -> float -> float

let addFloats: AddFloats =
    fun a1 a2 ->
        a1 + a2

let result2 = addFloats 10. 20.

///////////////////////////////////////////////////////////







