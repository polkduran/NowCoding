namespace NowCoding

module DecisionTrees =

    // https://en.wikipedia.org/wiki/ID3_algorithm
// http://stackoverflow.com/questions/1418829/help-needed-creating-a-binary-tree-given-truth-table
// https://blogs.msdn.microsoft.com/chrsmith/2009/10/31/awesome-f-decision-trees-part-i/
// http://clear-lines.com/blog/post/Decision-Tree-classification.aspx

    type TreeNode<'T, 'U> = 
        |Category of 'U * float
        |Choice of string * ('T -> obj) * (obj * TreeNode<'T, 'U>) list

    type Tree<'T, 'U> = |Root of TreeNode<'T, 'U>

    let private entropyInternal (categorySelector:'T -> 'U) (data:'T seq) =
        let log2 x = System.Math.Log(x, 2.0)
        let dataCount = data |> Seq.length

        let probByCategory = data 
                                |> Seq.groupBy categorySelector // seq<'U * seq<'T>>
                                |> Seq.map (fun (a, ga) -> ga |> Seq.length |> float) //seq<float>
                                |> Seq.map (fun pa -> pa/float(dataCount) ) //seq<float>
        let entr = probByCategory 
                    |> Seq.sumBy (fun px -> px * log2 px)
                    |> fun e -> -e
                
        entr, dataCount

    let entropy (categorySelector:'T -> 'U) (data:'T seq) = entropyInternal categorySelector data |> fst
  
    let gain (data:'T seq) (categorySelector:'T -> 'U) (featureSelector:('T -> obj)) =
        let dataEntr, dataCount = entropyInternal categorySelector data
        let partDataEntr = data 
                            |> Seq.groupBy featureSelector
                            |> Seq.map (fun (a, part) -> part, part |> Seq.length |> float)
                            |> Seq.map (fun (part, partCount) -> partCount/float(dataCount), entropy categorySelector part) // seq<float * float>
                            |> Seq.sumBy (fun (pt, ht) -> pt * ht)
    
        dataEntr-partDataEntr

    let classify (datum:'T) (tree:Tree<'T, 'U>) = 
        let rec chooseNode feature (selector:('T -> obj)) (nodes:( obj * TreeNode<'T, 'U>) list) =
            match nodes with
            | (value, node)::_ when selector(datum) = value -> node
            | c::r -> chooseNode feature selector r
            | [] -> failwith <| sprintf "no node for feature %s and datum %A" feature datum

        let rec classifyInt (node:TreeNode<'T, 'U>) =
            match node with
            | Category(category, prob) -> category
            | Choice(feature, selector, nodes) -> chooseNode feature selector nodes |> classifyInt

        let root = match tree with |Root(r) -> r
        let value = classifyInt root
        value

    let trainID3 (data: 'T seq) (categorySelector:'T -> 'U) (features: (string * ('T -> obj)) list) = 
        let rec getOptFeature (feature:(string * ('T -> obj) * float) option) (restFeatures: (string * ('T -> obj)) list) restData acc =
            match feature, restFeatures with
            | None, [] -> failwith "max with empty"
            | None, (n,f)::[] -> n, f, acc
            | None, (n,f)::r -> getOptFeature (Some(n, f, gain restData categorySelector f))  r restData acc
            | Some(n, f, gainF), [] -> n, f, acc
            | Some(n1, f1, gainF1), (n2,f2)::r -> 
                                let gainF2 = gain restData categorySelector f2
                                if gainF1 > gainF2 then
                                    getOptFeature (Some(n1,f1,gainF1)) r restData ((n2,f2)::acc)
                                else
                                    getOptFeature (Some(n2,f2,gainF2)) r restData ((n1,f1)::acc)

        let getMaxCatProb featureData =
            let len = Seq.length >> float
            let dataCount = featureData |> len
            let featDataByCatProb = featureData 
                                    |> Seq.groupBy categorySelector
                                    |> Seq.map (fun (cat, d) -> cat, (d |> len) / dataCount)
            let maxCat = featDataByCatProb |> Seq.maxBy snd //|> fst
            maxCat
        
        let rec trainID3Int (restData: 'T seq) (restFeatures: (string * ('T -> obj)) list) =
            match restFeatures with
            | [] -> failwith "emtpy features"
            | (feature, featureSelector)::[] ->  
                    let partData = restData 
                                |> Seq.groupBy featureSelector
                                |> Seq.map (fun (feature, featData) -> feature, getMaxCatProb featData) 
                                |> Seq.map (fun (feature, cat) -> feature, Category(cat)) |> Seq.toList
                    Choice(feature, featureSelector, partData)
            | _ -> let bestFeatureName, bestFeatureSel, others =  getOptFeature None restFeatures restData []
                   let uniqueCategory = Seq.groupBy categorySelector >> Seq.length >> (=)1
                   let catPart, choicePart = restData 
                                                |> Seq.groupBy bestFeatureSel |> Seq.toList
                                                |> List.partition (snd >> uniqueCategory)
                   let catNodes = catPart 
                                    |> Seq.map (fun (featVal, featData) -> featVal, Category(featData |> Seq.head |> categorySelector, 1.0))
                                    |> Seq.toList
                   let choiceNodes = choicePart 
                                        |> Seq.map (fun (featVal, featData) -> featVal, trainID3Int featData others )
                                        |> Seq.toList
                   Choice(bestFeatureName, bestFeatureSel, catNodes@choiceNodes)

        let rootNode = match features with
                        | [] -> let cat = getMaxCatProb data
                                Category(cat)
                        | _ -> trainID3Int data features
        Root(rootNode)

    let displayTree (tree:Tree<'T, 'U>) =
        let append (sb:System.Text.StringBuilder) (s:string) = sb.Append(s) |> ignore
        let appendLine (sb:System.Text.StringBuilder) (s:string) = sb.Append("\n"+s) |> ignore
        let tabs nb = String.replicate nb "\t"

        let rec displayNode (node:TreeNode<'T,'U>) (acc:System.Text.StringBuilder) lev c =
            match node with
            | Category(cat, prob) -> append acc <| sprintf " > (%d, %A, %f)" lev cat prob
            | Choice(feature,_,nodes) ->
                    appendLine acc <| sprintf "%s[feature:%s]" (tabs lev) feature
                    nodes 
                        |> List.iter (fun (value, node) -> 
                                                appendLine acc <| sprintf "%s(val:%A)" (tabs (lev+1)) value 
                                                displayNode node acc (lev+1) (c+1))
                
    
        let root = match tree with |Root(r) -> r
        let sb = new System.Text.StringBuilder()
        displayNode root sb 0 0
        sb.ToString()





