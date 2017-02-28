#load "Lib.fs"
open NowCoding.DecisionTrees

// http://archive.ics.uci.edu/ml/datasets/Lenses
type Pat = {Id:int; Age:int; Presc:int; Ast:int; Tear:int; Res:int}
let data = [
            {Id=1;  Age=1; Presc=1; Ast=1; Tear=1; Res=3};
            {Id=2;  Age=1; Presc=1; Ast=1; Tear=2; Res=2};
            {Id=3;  Age=1; Presc=1; Ast=2; Tear=1; Res=3};
            {Id=4;  Age=1; Presc=1; Ast=2; Tear=2; Res=1};
            {Id=5;  Age=1; Presc=2; Ast=1; Tear=1; Res=3};
            {Id=6;  Age=1; Presc=2; Ast=1; Tear=2; Res=2};
            {Id=7;  Age=1; Presc=2; Ast=2; Tear=1; Res=3};
            {Id=8;  Age=1; Presc=2; Ast=2; Tear=2; Res=1};
            {Id=9;  Age=2; Presc=1; Ast=1; Tear=1; Res=3};
            {Id=10; Age=2; Presc=1; Ast=1; Tear=2; Res=2};
            {Id=11; Age=2; Presc=1; Ast=2; Tear=1; Res=3};
            {Id=12; Age=2; Presc=1; Ast=2; Tear=2; Res=1};
            {Id=13; Age=2; Presc=2; Ast=1; Tear=1; Res=3};
            {Id=14; Age=2; Presc=2; Ast=1; Tear=2; Res=2};
            {Id=15; Age=2; Presc=2; Ast=2; Tear=1; Res=3};
            {Id=16; Age=2; Presc=2; Ast=2; Tear=2; Res=3};
            {Id=17; Age=3; Presc=1; Ast=1; Tear=1; Res=3};
            {Id=18; Age=3; Presc=1; Ast=1; Tear=2; Res=3};
            {Id=19; Age=3; Presc=1; Ast=2; Tear=1; Res=3};
            {Id=20; Age=3; Presc=1; Ast=2; Tear=2; Res=1};
            {Id=21; Age=3; Presc=2; Ast=1; Tear=1; Res=3};
            {Id=22; Age=3; Presc=2; Ast=1; Tear=2; Res=2};
            {Id=23; Age=3; Presc=2; Ast=2; Tear=1; Res=3};
            {Id=24; Age=3; Presc=2; Ast=2; Tear=2; Res=3}
          ]

let catSelector = (fun p -> p.Res)
let featureSelectors = [
                    "age",(fun p -> p.Age :> obj);
                    "presc",(fun p -> p.Presc :> obj);
                    "ast",(fun p -> p.Ast :> obj);
                    "tear",(fun p -> p.Tear :> obj);
                    ]

let tree = trainID3 data catSelector featureSelectors
