cd $HADOOP_PREFIX

#bin/hdfs dfsadmin -safemode leave

bin/hdfs dfs -mkdir /hdfs

bin/hdfs dfs -mkdir /hdfs/race

bin/hdfs dfs -copyFromLocal /share/data/race/* /hdfs/race/

bin/hdfs fs -rm -r /hdfs-output/race

bin/hadoop jar /usr/local/hadoop-2.6.0/share/hadoop/tools/lib/hadoop-streaming-2.6.0.jar -input "/hdfs/race" -output "/hdfs-output/race" -mapper "mono /share/Debug/Mapper.exe"

bin/hdfs dfs -cat /hdfs-output/race/*