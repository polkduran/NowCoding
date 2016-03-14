// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Lib.fs"
open NowCoding


type Gender = |Male |Female
type Row = {Gender:Gender; Phd:bool; IsEvil:bool}

let data = [
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=true} ;
            {Gender=Male   ;Phd=false ;IsEvil=true} ;
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male   ;Phd=true  ;IsEvil=true} ;
            {Gender=Male   ;Phd=true  ;IsEvil=true} ;
            {Gender=Female ;Phd=true  ;IsEvil=true} ;
            {Gender=Female ;Phd=true  ;IsEvil=true} ;
            {Gender=Male   ;Phd=true  ;IsEvil=false};
            ]
let genderSelector = fun (r:Row) -> r.Gender :> obj
let phdSelector = fun (r:Row) -> r.Phd :> obj
let isEvilSelector = fun (r:Row) -> r.IsEvil

let e = DecisionTrees.entropy isEvilSelector data 
let g = DecisionTrees.gain data isEvilSelector genderSelector 
let g' = DecisionTrees.gain data isEvilSelector phdSelector  

let evTree = DecisionTrees.trainID3 data isEvilSelector ["phd",phdSelector; "gender",genderSelector]


type Mov = {Action:bool; SciFi:bool; Actor:string}

let catSelector = fun (m:Mov) -> m.Actor
let actionSelector = fun (m:Mov) -> m.Action :> obj
let sciFiSelector = fun (m:Mov) -> m.SciFi :> obj

let root = DecisionTrees.Choice(
                "scifi", sciFiSelector, [
                        (true :> obj, DecisionTrees.Category("A", 1.0))
                        (false:> obj, DecisionTrees.Choice("action", actionSelector, [
                                                                  (true :> obj, DecisionTrees.Category("S", 1.0));
                                                                  (false:> obj, DecisionTrees.Category("A", 1.0))
                        ]
                        ))
                ])

let d = [|
            {Action=true;SciFi=false;Actor="S"};
            {Action=true;SciFi=false;Actor="S"};
            {Action=false;SciFi=false;Actor="A"};
            {Action=true;SciFi=true;Actor="A"};
            {Action=true;SciFi=true;Actor="A"}
        |]

let tree = DecisionTrees.trainID3 d catSelector ["action",actionSelector; "scifi",sciFiSelector]

// f1: [a1; a2; a3]
// f2: [b1; b2; b3]
// y : [y1; y2; y3]
let data2 = [
        ("a1", "b1", "y1");
        ("a1", "b2", "y2");
        ("a2", "b3", "y3");
        ("a3", "b3", "y1");
]

let f1 = fun (x,_,_) -> x :>obj
let f2 = fun (_,x,_) -> x :>obj
let y  = fun (_,_,y) -> y

let tree2 = DecisionTrees.trainID3 data2 y ["f1", f1;"f2", f2;]

let vals = [|
            data2 |> List.map (f1>>string) |> List.distinct;
            data2 |> List.map (f2>>string) |> List.distinct;
            |]
//let allCombs = [|
//                for x1 in vals.[0] do
//                    for x2 in vals.[1] -> (x1,x2,"")|]

let allCombs = [|
                ("a1", "b1", ""); 
                ("a1", "b2", ""); 
                ("a1", "b3", ""); 
                ("a2", "b1", "");
                ("a2", "b2", ""); 
                ("a2", "b3", ""); 
                ("a3", "b1", ""); 
                ("a3", "b2", "");
                ("a3", "b3", "")|]
let allClassified = allCombs |> Array.map (fun x -> DecisionTrees.classify x tree2)

printf "%s" (DecisionTrees.displayTree tree2)
               



