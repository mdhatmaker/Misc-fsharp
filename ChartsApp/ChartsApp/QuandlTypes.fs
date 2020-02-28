module QuandlTypes

/// https://github.com/lppkarl/Quandl.NET

open System


/// round as 2-decimal float (or return " " if None)
let strf x =
    match x with
    | Some x -> sprintf "%.2f" x 
    | None -> " "

/// round as integer (or return " " if None)
let stri x =
    match x with
    | Some x -> sprintf "%.0f" x 
    | None -> " "


// https://stackoverflow.com/questions/791706/how-do-i-customize-output-of-a-custom-type-using-printf
//[<StructuredFormatDisplay("stock {Date} {Open} {High} {Low} {Close} {Volume} (ok)")>]

[<StructuredFormatDisplay("{Date} {Open} {High} {Low} {Close} {Volume} | {ExDividend} {SplitRatio} | {AdjOpen} {AdjHigh} {AdjLow} {AdjClose} {AdjVolume}")>]
type FuturesData =
    { Date:string
      Open:float
      High:float
      Low:float
      Close:float
      Volume:float
      ExDividend:float
      SplitRatio:float
      AdjOpen:float
      AdjHigh:float
      AdjLow:float
      AdjClose:float
      AdjVolume:float }
    override m.ToString() = sprintf "%s o:%.2f h:%.2f l:%.2f c:%.2f v:%.0f  exdiv:%.2f split:%.2f  ao:%.2f ah:%.2f al:%.2f ac:%.2f av:%.0f" m.Date m.Open m.High m.Low m.Close m.Volume m.ExDividend m.SplitRatio m.AdjOpen m.AdjHigh m.AdjLow m.AdjClose m.AdjVolume

[<StructuredFormatDisplay("{Date} {Open} {High} {Low} {Settle} {Change} | {Wave} {Volume} | {PrevDayOpenInterest} {EFPVolume} {EFSVolume} {BlockVolume}")>]
type ContinuousData =
    { Date:string
      Open:float option
      High:float option
      Low:float option
      Settle:float option
      Change:float option
      Wave:float option
      Volume:float option
      PrevDayOpenInterest:float option
      EFPVolume:float option
      EFSVolume:float option
      BlockVolume:float option }
    override m.ToString() = sprintf "%s o:%s h:%s l:%s s:%s chg:%s  wave:%s v:%s  oi:%s efpv:%s efsv:%s bv:%s" m.Date (strf m.Open) (strf m.High) (strf m.Low) (strf m.Settle) (strf m.Change) (strf m.Wave) (strf m.Volume) (strf m.PrevDayOpenInterest) (strf m.EFPVolume) (strf m.EFSVolume) (strf m.BlockVolume)

// Under $100_000; $100_000 - $199_999; $200_000 - $299_999; $300_000 - $399_999;
// $400_000 - $499_999; $500_000 +; DK; NA; 25th Percentile; Median; 75th Percentile;
// Interquartile Range (75th-25th);
[<StructuredFormatDisplay("{Date} {PriceUnder100} {Price100to199} {Price200to299} {Price300to399} {Price400to499} {PriceOver500} | {DK} {NA} | {Pct25} {Median} {Pct75} {InterquartileRange}")>]
type UmichConsumerData =
    { Date:string
      PriceUnder100:float option
      Price100to199:float option
      Price200to299:float option
      Price300to399:float option
      Price400to499:float option
      PriceOver500:float option
      DKNA:float option
      Pct25:float option
      Median:float option
      Pct75:float option
      InterquartileRange:float option }
      //override m.ToString() = sprintf "%s <100:%s 100-199:%s 200-299:%s 300-399:%s 400-499:%s >500:%s  DK:%s NA:%s  25pct:%s median:%s 75pct:%s IQRng:%s" m.Date (strf m.PriceUnder100) (strf m.Price100to199) (strf m.Price200to299) (strf m.Price300to399) (strf m.Price400to499) (strf m.PriceOver500) (strf m.DK) (strf m.NA) (strf m.Pct25) (strf m.Median) (strf m.Pct75) (strf m.InterquartileRange)
    override m.ToString() = sprintf "%s <100:%s 100-199:%s 200-299:%s 300-399:%s 400-499:%s >500:%s  DK/NA:%s  25pct:%s median:%s 75pct:%s IQRng:%s" m.Date (stri m.PriceUnder100) (stri m.Price100to199) (stri m.Price200to299) (stri m.Price300to399) (stri m.Price400to499) (stri m.PriceOver500) (stri m.DKNA) (stri m.Pct25) (stri m.Median) (stri m.Pct75) (stri m.InterquartileRange)

[<StructuredFormatDisplay("{date} {value}")>]
type ZillowData =
    { date:string
      value:float option }
    override m.ToString() = sprintf "%s value:%s" m.date (strf m.value)




let tos (o:obj) = (string o)
     
let tof (o:obj) = float (string o)
     
let trytof (o:obj) =
    let (success,x) = Double.TryParse(string o)
    if success then Some x
    else None

let parseFuturesData (item:obj[]) =
    { Date=(tos item.[0]); Open=(tof item.[1]); High=(tof item.[2]); Low=(tof item.[3]); Close=(tof item.[4]); Volume=(tof item.[5]);
    ExDividend=(tof item.[6]); SplitRatio=(tof item.[7]);
    AdjOpen=(tof item.[8]); AdjHigh=(tof item.[9]); AdjLow=(tof item.[10]); AdjClose=(tof item.[11]); AdjVolume=(tof item.[12])
    }

let parseContinuousData (item:obj[]) =
    { Date=(tos item.[0]); Open=(trytof item.[1]); High=(trytof item.[2]); Low=(trytof item.[3]); Settle=(trytof item.[4]);
    Change=(trytof item.[5]); Wave=(trytof item.[6]); Volume=(trytof item.[7]); PrevDayOpenInterest=(trytof item.[8]);
    EFPVolume=(trytof item.[9]); EFSVolume=(trytof item.[10]); BlockVolume=(trytof item.[11])
    }

let parseUmichConsumerData (item:obj[]) =
    { Date=(tos item.[0]); PriceUnder100=(trytof item.[1]); Price100to199=(trytof item.[2]); Price200to299=(trytof item.[3]); Price300to399=(trytof item.[4]); Price400to499=(trytof item.[5]); PriceOver500=(trytof item.[6]);
    DKNA=(trytof item.[7]);
    Pct25=(trytof item.[8]); Median=(trytof item.[9]); Pct75=(trytof item.[10]); InterquartileRange=(trytof item.[11]);
    }

let parseZillowData (item:obj[]) =
    { date=(tos item.[0]); value=(trytof item.[1]);
    }



