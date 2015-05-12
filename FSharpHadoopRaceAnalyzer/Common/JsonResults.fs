module JsonResults

open FSharp.Data

type result = JsonProvider< """{ "groupName" : "Overall Checkpoint 1", "bib" : 4231, "time" : "00:00:02.8750000", "age" : 21 }""", RootName="result" >
