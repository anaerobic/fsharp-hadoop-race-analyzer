module Mapper

open JsonTypes
open Newtonsoft.Json
open System

let private parseJson (value : string) = 
    try 
        let splits = value.Split('\t')
        let json = splits.[1] |> resultJson.Parse
        Some(json.GroupName, 
             JsonConvert.SerializeObject({ age = json.Age
                                           time = json.Time.TimeOfDay }))
    with :? ArgumentException -> None

let Map(value : string) = parseJson value
