module DecisionTrees
﻿
// https://en.wikipedia.org/wiki/ID3_algorithm
// http://stackoverflow.com/questions/1418829/help-needed-creating-a-binary-tree-given-truth-table
// https://blogs.msdn.microsoft.com/chrsmith/2009/10/31/awesome-f-decision-trees-part-i/

let entropy (data:seq<'T>) (selector:'T -> 'U) =
    let count = data |> Seq.length |> float
    let splitDataByFeature = data 
                                |> Seq.groupBy selector // seq<'U * seq<'T>>
                                |> Seq.map (fun (x, gx) -> x, gx |> Seq.toArray) //seq<'U * 'T[]>
    
    let partitionProb = splitDataByFeature |> Seq.map (fun (x, gx) -> x, float(gx.Length)/count ) //seq<'U * float>
    let entr = partitionProb 
                |> Seq.map snd // seq<float>
                |> Seq.sumBy (fun px -> px * Math.Log(px, 2.0))
                |> fun e -> -e
                
    entr, splitDataByFeature
  
let gain (data:seq<'T>) (selector:'T -> 'U) (selector2:('T -> 'V)) =
    let count = data |> Seq.length |> float
    let dataEntr, splitDataByFeature = entropy data selector
    
    let splitDataEntr = splitDataByFeature 
                        |> Seq.map snd //seq<'T[]>
                        |> Seq.map (fun part -> float(part.Length)/count, entropy part selector2  |> fst ) // seq<float * float>
                        |> Seq.sumBy (fun (pt,ht) -> pt*ht)
    
    dataEntr-splitDataEntr

