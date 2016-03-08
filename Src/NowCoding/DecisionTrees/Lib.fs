module DecisionTrees
open System

let entropy (data:seq<'T>) (selector:'T -> 'U) =
    let count = data |> Seq.length |> float
    let entr = data 
                    |> Seq.groupBy selector
                    |> Seq.map (snd >> Seq.length >> float >> fun c -> c/count)
                    |> Seq.sumBy (fun px -> px*Math.Log(px,2.0))
    -entr

