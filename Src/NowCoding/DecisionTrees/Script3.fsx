#load "Lib.fs"
open NowCoding

let data =
    let file = @"C:\Users\pablo\Desktop\nursery.data 2.txt"
    System.IO.File.ReadLines(file) 
        //|> Seq.filter (fun line -> System.String.IsNullOrWhiteSpace(line))
        |> Seq.map (fun line -> line.Split(','))


let catSelector = fun (d:string[]) -> d.[8]
let getFeature i = fun (d:string[]) -> d.[i] :> obj

let features = [
                "parents  ", getFeature 0;
                "has_nurs ", getFeature 1;
                "form     ", getFeature 2;
                "children ", getFeature 3;
                "housing  ", getFeature 4;
                "finance  ", getFeature 5;
                "social   ", getFeature 6;
                "health   ", getFeature 7;
]

let vals = features 
                |> Seq.map (fun (feat,sel) -> data |> Seq.map sel |> Seq.distinct |> Seq.toList)
                |> Seq.toList

let cats = data |> Seq.map catSelector     |> Seq.distinct |> Seq.toArray

let tree = DecisionTrees.trainID3 data catSelector features

//
//let vals = [
//            ["usual"; "pretentious"; "great_pret"                              ]
//            ["proper"; "less_proper"; "improper"; "critical"; "very_crit"      ]
//            ["complete"; "completed"; "incomplete"; "foster"                   ]
//            ["1"; "2"; "3"; "more"                                             ]
//            ["convenient"; "less_conv"; "critical"                             ]
//            ["convenient"; "inconv"                                            ]
//            ["nonprob"; "slightly_prob"; "problematic"                         ]
//            ["recommended"; "priority"; "not_recom"                            ]
//            ]

let feats = features |> List.map fst |> Array.ofList 
let subs = seq{
                for f1 in vals.[0] do
                for f2 in vals.[1] do
                for f3 in vals.[2] do
                for f4 in vals.[3] do
                for f5 in vals.[4] do
                for f6 in vals.[5] do
                for f7 in vals.[6] do
                for f8 in vals.[7] do
                    yield [|
                        f1.ToString();
                        f2.ToString();
                        f3.ToString();
                        f4.ToString();
                        f5.ToString();
                        f6.ToString();
                        f7.ToString();
                        f8.ToString();
                    |]         
            }


let res = subs |> Seq.map (fun s -> DecisionTrees.classify s tree) |> Seq.toArray
System.IO.File.WriteAllLines(@"C:\Users\pablo\Desktop\nursery.mine.txt",res)

 //|Category of 'U * float
 //|Choice of string * ('T -> obj) * (obj * TreeNode<'T, 'U>) list

let rec probs node acc =
    match node with
    | DecisionTrees.Category(c,p) -> p::acc
    | DecisionTrees.Choice(label, sel, nodes) -> 
            let pbs = nodes |> List.collect (fun (o,n) -> probs n acc)
            acc@pbs
            

let root = match tree with |DecisionTrees.Root(r) -> r
let ps = probs root [] |> List.distinct

// [|"usual"; "proper"; "complete"; "1"; "convenient"; "inconv"; "nonprob"; "recommended"|]