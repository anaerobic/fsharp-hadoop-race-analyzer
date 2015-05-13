# fsharp-hadoop-race-analyzer
A simple F# service to execute a Hadoop job using Hadoop Streaming

##Usage
Note: I am currently just trying to get this to run, so I am doing some evil..

Create a ~/hadoop/share/race/data directory

Create a ~/hadoop/share/race/Debug directory

Build the solution and copy all Mapper & Reducer bin/Debug contents to ~/hadoop/share/race/Debug

Create a file ~/hadoop/share/data/results with some data like the following:
```
0	{"groupName":"female Checkpoint 0","bib":4911,"time":"00:00:02.8750000","age":50}
1	{"groupName":"female 48-53 Checkpoint 0","bib":4911,"time":"00:00:02.8750000","age":50}
2	{"groupName":"Overall Checkpoint 0","bib":1115,"time":"00:00:07.2700000","age":26}
3	{"groupName":"female Checkpoint 0","bib":1115,"time":"00:00:07.2700000","age":26}
4	{"groupName":"female 23-28 Checkpoint 0","bib":1115,"time":"00:00:07.2700000","age":26}
5	{"groupName":"Overall Checkpoint 0","bib":1511,"time":"00:00:10.2890000","age":34}
6	{"groupName":"female Checkpoint 0","bib":1511,"time":"00:00:10.2890000","age":34}
```

Spin up Hadoop in Docker with Mono and FSharp installed with something like this:
```
docker run --rm -p 8088:8088 -p 8042:8042 -v ~/hadoop/share:/share -i -t anaerobic/hadoop-fsharp /etc/bootstrap.sh -bash
```

Run inside Hadoop with something like this:
```
cd $HADOOP_PREFIX

bin/hdfs dfs -mkdir /hdfs

bin/hdfs dfs -mkdir /hdfs/race

bin/hdfs dfs -copyFromLocal /share/race/data/results /hdfs/race/results

bin/hdfs dfs -rm -r /hdfs-output/race

bin/hadoop jar /usr/local/hadoop-2.6.0/share/hadoop/tools/lib/hadoop-streaming-2.6.0.jar -input "/hdfs/race" -output "/hdfs-output/race" -mapper "mono /share/race/Debug/Mapper.exe" -reducer "mono /share/race/Debug/Reducer.exe"
```

And we can check the output with:
```
bin/hdfs dfs -cat /hdfs-output/race/*

Overall Checkpoint 0    {"minAge":26,"maxAge":34,"avgAge":30}
female 23-28 Checkpoint 0       {"minAge":26,"maxAge":26,"avgAge":26}
female 48-53 Checkpoint 0       {"minAge":50,"maxAge":50,"avgAge":50}
female Checkpoint 0     {"minAge":26,"maxAge":50,"avgAge":36}
```

Which is reduced from the following Mapper output:
```
Overall Checkpoint 0    {"age":34,"time":"00:00:10.2890000"}
Overall Checkpoint 0    {"age":26,"time":"00:00:07.2700000"}
female 23-28 Checkpoint 0       {"age":26,"time":"00:00:07.2700000"}
female 48-53 Checkpoint 0       {"age":50,"time":"00:00:02.8750000"}
female Checkpoint 0     {"age":26,"time":"00:00:07.2700000"}
female Checkpoint 0     {"age":50,"time":"00:00:02.8750000"}
female Checkpoint 0     {"age":34,"time":"00:00:10.2890000"}
```

Feel free to poke around in the Hadoop Web UI at:
```
http://<host_ip>:8088/cluster
```
