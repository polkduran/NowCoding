module DecisionTrees﻿

// https://en.wikipedia.org/wiki/ID3_algorithm
// http://stackoverflow.com/questions/1418829/help-needed-creating-a-binary-tree-given-truth-table
// https://blogs.msdn.microsoft.com/chrsmith/2009/10/31/awesome-f-decision-trees-part-i/
// http://clear-lines.com/blog/post/Decision-Tree-classification.aspx

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
        |Category of 'U
        |Node of ('T -> obj) * (obj * TreeNode<'T, 'U>) list

type Tree<'T, 'U> = |Root of TreeNode<'T, 'U>

let classify (datum:'T) (tree:Tree<'T, 'U>) = 
    let rec chooseNode (selector:('T -> obj)) (nodes:( obj * TreeNode<'T, 'U>) list) =
        match nodes with
        | (value, node)::_ when selector(datum) = value -> Some(node)
        | c::r -> chooseNode selector r
        | [] -> None

    let rec classifyInt (node:TreeNode<'T, 'U>) =
        match node with
        | Category(category) -> category
        | Node(selector, nodes) -> match chooseNode selector nodes with
                                   | Some(node) -> classifyInt node
                                   | None -> failwith "no node"

    let root = match tree with |Root(r) -> r
    let value = classifyInt root
    value

let trainID3 (data: 'T seq) (categorySelector:'T -> 'U) (features: seq<'T -> obj>) = 
    
    let rec trainID3Int restData (featureSelector:'T -> obj) (currentNode:TreeNode<'T, 'U>) (otherFeatures: ('T -> obj) list) =
        let restDataCount = restData |> Seq.length |> float
        let getDataDistribution featureData =
            let featDataByCatProb = featureData 
                                    |> Seq.groupBy categorySelector
                                    |> Seq.map (fun (cat, d) -> cat, (d |> Seq.length |> float) / restDataCount)
            featDataByCatProb
        
        match otherFeatures with
        | [] -> let partData = restData 
                                    |> Seq.groupBy featureSelector
                                    |> Seq.map (fun (feature, featData) -> feature, getDataDistribution featData) 
                                    |> Seq.map (fun (feature, featDataByCatProb) -> feature, featDataByCatProb |> Seq.maxBy snd |> fst ) // can keep the probability
                                    |> Seq.map (fun (feature, cat) -> feature, Category(cat)) |> Seq.toList
                Node(featureSelector, partData)
    ()

               
type Mov = {Action:bool; SciFi:bool; Actor:string}

let catSelector = fun (m:Mov) -> m.Actor
let actionSelector = fun (m:Mov) -> m.Action :> obj
let sciFiSelector = fun (m:Mov) -> m.SciFi :> obj

let root = Node(
                sciFiSelector, [
                        (true :> obj, Category("A"))
                        (false:> obj, Node(actionSelector, [
                                            (true :> obj, Category("S"));
                                            (false:> obj, Category("A"))
                        ]
                        ))
                ])
let tree = Root(root)

let t1 = {Action=true;  SciFi=true;  Actor=""}
let t2 = {Action=true;  SciFi=false; Actor=""}
let t3 = {Action=false; SciFi=true;  Actor=""}
let t4 = {Action=false; SciFi=false; Actor=""}


