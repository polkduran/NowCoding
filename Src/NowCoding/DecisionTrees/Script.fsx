// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Lib.fs"

type Gender = |Male |Female
type Row = {Gender:Gender; Phd:bool; IsEvil:bool}

let data = [
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=true} ;
            {Gender=Male ;Phd=false ;IsEvil=true} ;
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Female ;Phd=false ;IsEvil=false};
            {Gender=Male ;Phd=true ;IsEvil=true} ;
            {Gender=Male ;Phd=true ;IsEvil=true} ;
            {Gender=Female ;Phd=true ;IsEvil=true} ;
            {Gender=Female ;Phd=true ;IsEvil=true} ;
            {Gender=Male ;Phd=true ;IsEvil=false};
            ]
let genderSelector = fun (r:Row) -> r.Gender :> obj
let phdSelector = fun (r:Row) -> r.Phd :> obj
let isEvilSelector = fun (r:Row) -> r.IsEvil

let e = ``DecisionTrees﻿``.entropy isEvilSelector data 
let g = ``DecisionTrees﻿``.gain data isEvilSelector genderSelector 
let g' = ``DecisionTrees﻿``.gain data isEvilSelector phdSelector  



