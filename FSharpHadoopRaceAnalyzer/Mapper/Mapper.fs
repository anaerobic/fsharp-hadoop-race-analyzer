module Mapper

open JsonResults
open System

let private parseJson (value : string) = 

    try
        let splits = value.Split('\t')
        let json = splits.[1] |> result.Parse
        Some(json.GroupName, json)
    with
    | :? ArgumentException -> None

let Map (value: string) =
    parseJson value
