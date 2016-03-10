module DecisionTrees﻿
// https://en.wikipedia.org/wiki/ID3_algorithm
// http://stackoverflow.com/questions/1418829/help-needed-creating-a-binary-tree-given-truth-table
// https://blogs.msdn.microsoft.com/chrsmith/2009/10/31/awesome-f-decision-trees-part-i/

let private entropyInternal (categorySelector:'T -> 'U) (data:'T seq) =
    let log2 x = System.Math.Log(x, 2.0)
    let dataCount = data |> Seq.length

    let probByCategory = data 
                            |> Seq.groupBy categorySelector // seq<'U * seq<'T>>
                            |> Seq.map (fun (a, ga) -> ga |> Seq.length |> float) //seq<float>
                            |> Seq.map (fun pa -> pa/float(dataCount) ) //seq<float>
    // partDataByFeature : 
    let entr = probByCategory 
                |> Seq.sumBy (fun px -> px * log2 px)
                |> fun e -> -e
                
    entr, dataCount

let entropy (categorySelector:'T -> 'U) (data:'T seq) = entropyInternal categorySelector data |> fst
  
let gain (data:'T seq) (categorySelector:'T -> 'U) (partSelector:('T -> obj)) =
    let dataEntr, dataCount = entropyInternal categorySelector data
    let partDataEntr = data 
                        |> Seq.groupBy partSelector
                        |> Seq.map (fun (a, part) -> part, part |> Seq.length |> float)
                        |> Seq.map (fun (part, partCount) -> partCount/float(dataCount), entropy categorySelector part) // seq<float * float>
                        |> Seq.sumBy (fun (pt, ht) -> pt * ht)
    
    dataEntr-partDataEntr

type TreeNode<'T, 'U> = 
        |Category of ('T -> 'U)
        |Node of ('T -> obj) * (obj * TreeNode<'T, 'U>) list
type Tree<'T, 'U> = TreeNode<'T, 'U>

let classify (datum:'T) (tree:Tree<'T, 'U>) = 
    let rec chooseNode (selector:('T -> obj)) (nodes:( obj * TreeNode<'T, 'U>) list) =
        match nodes with
        | (value, node)::_ when selector(datum) = value -> Some(node)
        | c::r -> chooseNode selector r
        | [] -> None

    let rec classifyInt (node:TreeNode<'T, 'U>) =
        match node with
        | Category(categorySelector) -> categorySelector datum
        | Node(selector, nodes) -> match chooseNode selector nodes with
                                   | Some(node) -> classifyInt node
                                   | None -> failwith "no node"

    let value = classifyInt tree
    value

//let trainID3 (data: 'T seq) (categorySelector:'T -> 'U) (features: seq<'T -> obj>) = 
    

