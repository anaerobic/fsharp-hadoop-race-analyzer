﻿module Reducer

open JsonTypes
open Newtonsoft.Json
open System

let Reduce (key : string) (values : seq<string>) = 
    let initState = (99, 0, 0, 0)
    
    let (minAge, maxAge, totalAge, totalCount) = 
        values
        |> Seq.map ageTimeJson.Parse
        |> Seq.fold 
               (fun (minAge, maxAge, totalAge, totalCount) el -> 
               (min minAge el.Age, max maxAge el.Age, totalAge + el.Age, totalCount + 1)) (99, 0, 0, 0)
    Some(JsonConvert.SerializeObject({ minAge = minAge
                                       maxAge = maxAge
                                       avgAge = totalAge / totalCount }))
