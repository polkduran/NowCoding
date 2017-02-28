module DecisionTrees =

    type Tree = 
        | Conclusion of string 
        | Choice of string * (string * Tree) []

    let prop count total = (float)count / (float)total

    let inspect dataset =
        let header, (data: 'a [][]) = dataset
        let rows = data |> Array.length
        let columns = header |> Array.length
        header, data, rows, columns

    let h vector =
        let size = vector |> Array.length
        vector 
        |> Seq.groupBy (fun e -> e)
        |> Seq.sumBy (fun e ->
            let count = e |> snd |> Seq.length
            let p = prop count size
            - p * log p)

    let entropy dataset =
        let _, data, _, cols = inspect dataset
        data
        |> Seq.map (fun row -> row.[ cols-1 ])
        |> Seq.toArray
        |> h

    let remove i vector =
        let size = vector |> Array.length
        Array.append vector.[ 0 .. i-1 ] vector.[ i+1 .. size-1 ]

    let split dataset i =
        let hdr, data, _, _ = inspect dataset
        remove i hdr,
        data
        |> Seq.groupBy (fun row -> row.[i])
        |> Seq.map (fun (label, group) -> 
            label,
            group |> Seq.toArray |> Array.map (remove i))

    let splitEntropy dataset i =
        let _, data, rows, cols = inspect dataset
        data
        |> Seq.groupBy(fun row -> row.[i])
        |> Seq.map (fun (label, group) -> 
            group 
            |> Seq.map (fun row -> row.[cols - 1]) 
            |> Seq.toArray)
        |> Seq.sumBy (fun subset -> 
            let p = prop (Array.length subset) rows
            p * h subset)

    let selectSplit dataset =
        let hdr, data, _, cols = inspect dataset
        if cols < 2 
        then None
        else
            let currentEntropy = entropy dataset      
            let feature =
                hdr.[0 .. cols - 2]
                |> Array.mapi (fun i f ->
                    (i, f), currentEntropy - splitEntropy dataset i)
                |> Array.maxBy (fun f -> snd f)
            if (snd feature > 0.0) then Some(fst feature) else None

    let majority dataset =
        let _, data, _, cols = inspect dataset
        data
        |> Seq.groupBy (fun row -> row.[cols-1])
        |> Seq.maxBy (fun (label, group) -> Seq.length group)
        |> fst

    let rec build dataset =
        match selectSplit dataset with
        | None -> Conclusion(majority dataset)
        | Some(feature) -> 
            let (index, name) = feature
            let (header, groups) = split dataset index
            let trees = 
                groups 
                |> Seq.map (fun (label, data) -> (label, build (header, data)))
                |> Seq.toArray
            Choice(name, trees)

    let rec classify subject tree =
        match tree with
        | Conclusion(c) -> c
        | Choice(label, options) ->
            let subjectState =
                subject
                |> Seq.tryFind(fun (key, value) -> key = label)
                |> Option.map snd
                |> function 
                   | Some(s) -> s
                   | None -> failwith <| sprintf "no key for label %s" label

            let nextTree = 
                        options
                        |> Array.tryFind (fun (option, tree) -> option = subjectState)
                        |> Option.map snd
                        |> function
                           | Some(t) -> t
                           | None -> failwith <| sprintf "no option for state %s" subjectState
            
            classify subject nextTree




// Nursery Dataset: http://archive.ics.uci.edu/ml/datasets/Nursery

let labels = [| "parents"; "has_nurs"; "form"; "children"; "housing"; "finance"; "social"; "health"; "Decision" |]

let nursery =
    let file = @"C:\Users\pablo\Desktop\nursery.data 2.txt"
    let fileAsLines =
        System.IO.File.ReadAllLines(file)
        |> Array.map (fun line -> line.Split(','))
    let dataset = 
        fileAsLines
        |> Array.map (fun line -> 
            [| line.[0]
               line.[1]; 
               line.[2]; 
               line.[3];
               line.[4]
               line.[5]
               line.[6]
               line.[7]
               line.[8] |])
    labels, dataset
let tree = DecisionTrees.build nursery

let vals = [|
            ["usual"; "pretentious"; "great_pret"                              ]
            ["proper"; "less_proper"; "improper"; "critical"; "very_crit"      ]
            ["complete"; "completed"; "incomplete"; "foster"                   ]
            ["1"; "2"; "3"; "more"                                             ]
            ["convenient"; "less_conv"; "critical"                             ]
            ["convenient"; "inconv"                                            ]
            ["nonprob"; "slightly_prob"; "problematic"                         ]
            ["recommended"; "priority"; "not_recom"                            ]
            |]
let features = Array.sub labels 0 8 
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
                        features.[0],f1;
                        features.[1],f2;
                        features.[2],f3;
                        features.[3],f4;
                        features.[4],f5;
                        features.[5],f6;
                        features.[6],f7;
                        features.[7],f8;
                    |]         
            }

let res = subs |> Seq.map (fun subject -> DecisionTrees.classify subject tree) |> Seq.toArray

System.IO.File.WriteAllLines(@"C:\Users\pablo\Desktop\nursery.not_mine.txt",res)

