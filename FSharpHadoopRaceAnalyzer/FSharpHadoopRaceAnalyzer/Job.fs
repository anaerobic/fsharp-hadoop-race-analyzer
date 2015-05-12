namespace FSharpHadoopRaceAnalyzer

open FSharp.Data
open Microsoft.Hadoop.MapReduce

type result = JsonProvider< """{ "groupName" : "Overall Checkpoint 1", "bib" : 4231, "time" : "00:00:02.8750000", "age" : 21 }""", RootName="result" >

type ResultsMapper() = 
    inherit MapperBase()
    override this.Map(inputLine : string, context : MapperContext) = 
        inputLine.Split [| '\t' |]
        |> Seq.mapi (fun i el -> el, i)
        |> Seq.filter (fun (el, i) -> i % 2 <> 0)
        |> Seq.map fst
        |> Seq.map result.Parse
        |> Seq.groupBy (fun x -> x.GroupName)
        |> Seq.map (fun (key, s) -> (key, Seq.length s))
        |> Seq.iter (fun (key, count) -> context.EmitKeyValue(key, count.ToString()))

type Job() = 
    inherit HadoopJob<ResultsMapper>()
    override this.Configure(context : ExecutorContext) = 
        let config = new HadoopJobConfiguration()
        config.InputPath <- "/hdfs"
        config.OutputFolder <- "/hdfs-output"
        config
