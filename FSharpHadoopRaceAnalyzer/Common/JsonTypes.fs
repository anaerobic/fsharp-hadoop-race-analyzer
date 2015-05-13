module JsonTypes

open FSharp.Data
open System

//type result = 
//    { groupName : string
//      bib : int32
//      time : TimeSpan
//      age : int32 }
// JsonProvider infers "time" as DateTime instead of TimeSpan, which sucks
type result = JsonProvider< """{ "groupName" : "Overall Checkpoint 1", "bib" : 4231, "time" : "00:00:02.8750000", "age" : 21 }""" >

type ageTime = 
    { age : int32
      time : TimeSpan }
// Cannot use JsonProvider because it outputs multiple lines per record, which sucks
//type ageTime = JsonProvider< """{ "age" : 21, "time" : "00:00:02.8750000" }""" >
