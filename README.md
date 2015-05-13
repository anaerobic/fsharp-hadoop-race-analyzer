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

bin/hdfs dfs -mkdir /hdfs/race

bin/hdfs dfs -copyFromLocal /share/race/data/results /hdfs/race/results

bin/hdfs dfs -rm -r /hdfs-output/race

bin/hadoop jar /usr/local/hadoop-2.6.0/share/hadoop/tools/lib/hadoop-streaming-2.6.0.jar -input "/hdfs/race" -output "/hdfs-output/race" -mapper "mono /share/race/Debug/Mapper.exe" -reducer "mono /share/race/Debug/Reducer.exe" -file "/share/race/Debug/Mapper.exe" -file "/share/race/Debug/Reducer.exe"
```

Get the following error:
```
2015-05-12 16:13:36,991 FATAL [IPC Server handler 3 on 57544] org.apache.hadoop.mapred.TaskAttemptListenerImpl: Task: attempt_1431461454847_0001_m_000001_0 - exited : java.lang.RuntimeException: PipeMapRed.waitOutputThreads(): subprocess failed with code 126
	at org.apache.hadoop.streaming.PipeMapRed.waitOutputThreads(PipeMapRed.java:322)
	at org.apache.hadoop.streaming.PipeMapRed.mapRedFinished(PipeMapRed.java:535)
	at org.apache.hadoop.streaming.PipeMapper.close(PipeMapper.java:130)
	at org.apache.hadoop.mapred.MapRunner.run(MapRunner.java:61)
	at org.apache.hadoop.streaming.PipeMapRunner.run(PipeMapRunner.java:34)
	at org.apache.hadoop.mapred.MapTask.runOldMapper(MapTask.java:450)
	at org.apache.hadoop.mapred.MapTask.run(MapTask.java:343)
	at org.apache.hadoop.mapred.YarnChild$2.run(YarnChild.java:163)
	at java.security.AccessController.doPrivileged(Native Method)
	at javax.security.auth.Subject.doAs(Subject.java:415)
	at org.apache.hadoop.security.UserGroupInformation.doAs(UserGroupInformation.java:1628)
	at org.apache.hadoop.mapred.YarnChild.main(YarnChild.java:158)
```

Or if we run it without the -file arguments like:
```
bin/hadoop jar /usr/local/hadoop-2.6.0/share/hadoop/tools/lib/hadoop-streaming-2.6.0.jar -input "/hdfs/race" -output "/hdfs-output/race" -mapper "mono /share/race/Debug/Mapper.exe" -reducer "mono /share/race/Debug/Reducer.exe"
```

We get this error:
```
15/05/12 19:25:54 INFO mapreduce.Job: Task Id : attempt_1431472291602_0005_m_000000_0, Status : FAILED
Error: java.lang.RuntimeException: PipeMapRed.waitOutputThreads(): subprocess failed with code 1
        at org.apache.hadoop.streaming.PipeMapRed.waitOutputThreads(PipeMapRed.java:322)
        at org.apache.hadoop.streaming.PipeMapRed.mapRedFinished(PipeMapRed.java:535)
        at org.apache.hadoop.streaming.PipeMapper.close(PipeMapper.java:130)
        at org.apache.hadoop.mapred.MapRunner.run(MapRunner.java:61)
        at org.apache.hadoop.streaming.PipeMapRunner.run(PipeMapRunner.java:34)
        at org.apache.hadoop.mapred.MapTask.runOldMapper(MapTask.java:450)
        at org.apache.hadoop.mapred.MapTask.run(MapTask.java:343)
        at org.apache.hadoop.mapred.YarnChild$2.run(YarnChild.java:163)
        at java.security.AccessController.doPrivileged(Native Method)
        at javax.security.auth.Subject.doAs(Subject.java:415)
        at org.apache.hadoop.security.UserGroupInformation.doAs(UserGroupInformation.java:1628)
        at org.apache.hadoop.mapred.YarnChild.main(YarnChild.java:158)
```

However, it works if we only run the Mapper with:
```
bin/hadoop jar /usr/local/hadoop-2.6.0/share/hadoop/tools/lib/hadoop-streaming-2.6.0.jar -input "/hdfs/race" -output "/hdfs-output/race" -mapper "mono /share/race/Debug/Mapper.exe"
```

And we can check the output with:
```
bin/hdfs dfs -cat /hdfs-output/race/*
```

Feel free to poke around aimlessly in the Hadoop Web UI at
```
http://<host_ip>:8088/cluster
```
