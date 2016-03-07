// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Lib.fs"
open DecisionTrees

let d = [(1.0,true);(2.0,true);(3.0,true);(2.0,true);(2.0,true)]

let efst = entropy d fst
let esnd = entropy d snd
