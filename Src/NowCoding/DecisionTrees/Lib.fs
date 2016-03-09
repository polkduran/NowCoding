module DecisionTrees﻿
// https://en.wikipedia.org/wiki/ID3_algorithm
// http://stackoverflow.com/questions/1418829/help-needed-creating-a-binary-tree-given-truth-table
// https://blogs.msdn.microsoft.com/chrsmith/2009/10/31/awesome-f-decision-trees-part-i/

let entropy (categorySelector:'T -> 'U) (data:seq<'T>) =
    let log2 x = System.Math.Log(x, 2.0)
    let dataCount = data |> Seq.length |> float

    let probByCategory = data 
                            |> Seq.groupBy categorySelector // seq<'U * seq<'T>>
                            |> Seq.map (fun (a, ga) -> ga |> Seq.length |> float) //seq<float>
                            |> Seq.map (fun pa -> pa/dataCount ) //seq<float>
    // partDataByFeature : 
    let entr = probByCategory 
                |> Seq.sumBy (fun px -> px * log2 px)
                |> fun e -> -e
                
    entr
  
let gain (data:seq<'T>) (categorySelector:'T -> 'U) (partSelector:('T -> 'V)) =
    let dataCount = data |> Seq.length |> float
    let dataEntr = entropy categorySelector data
    let partDataEntr = data 
                        |> Seq.groupBy partSelector
                        |> Seq.map (fun (a, part) -> part, part |> Seq.length |> float)
                        |> Seq.map (fun (part, partCount) -> partCount/dataCount, entropy categorySelector part) // seq<float * float>
                        |> Seq.sumBy (fun (pt, ht) -> pt * ht)
    
    dataEntr-partDataEntr

