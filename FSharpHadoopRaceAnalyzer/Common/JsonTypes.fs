module JsonTypes

open FSharp.Data
open System

type resultJson = JsonProvider< """{ "groupName" : "Overall Checkpoint 1", "bib" : 4231, "time" : "00:00:02.8750000", "age" : 21 }""" >

type ageTime = 
    { age : int32
      time : TimeSpan }

// JsonProvider infers "time" as DateTime instead of TimeSpan, which sucks
type ageTimeJson = JsonProvider< """{ "age" : 21, "time" : "00:00:02.8750000" }""" >

type minMaxAvg = 
    { minAge : int32
      maxAge : int32
      avgAge : int32 }
