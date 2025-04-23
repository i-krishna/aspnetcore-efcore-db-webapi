# Hadoop 3.0.0 Installation Guide for macOS

The installation includes HDFS (Distributed file storage systesm), YARN (for resource management) and MapReduce (for processing). 

## Example Cluster Setup
```
Node            Components
Master Node	   NameNode, ResourceManager (YARN)
Slave Node 1	DataNode, NodeManager (YARN)
Slave Node 2	DataNode, NodeManager (YARN)
Slave Node 3	DataNode, NodeManager (YARN)
```

https://hadoop.apache.org/ 

## Prerequisites
- Java JDK 8+ (Recommended: OpenJDK or Oracle JDK)
- Homebrew (for easy installation)
- Passwordless SSH (for localhost communication)

## Step 1: Install Hadoop & Configure SSH

### Install Hadoop via Homebrew (https://brew.sh/) 
```
brew install hadoop
```

### Set Up Passwordless SSH
```
ssh-keygen -t rsa -P '' -f ~/.ssh/id_rsa
cat ~/.ssh/id_rsa.pub >> ~/.ssh/authorized_keys
chmod 0600 ~/.ssh/authorized_keys
ssh localhost  # Test SSH (exit afterward)
```

## Step 2: Configure Hadoop

Navigate to Hadoop config files (path may vary):
```
cd /usr/local/Cellar/hadoop/3.0.0/libexec/etc/hadoop
```

1. Configure hadoop-env.sh
   
```
export JAVA_HOME=$(/usr/libexec/java_home)  # Auto-detects Java path
export HADOOP_HOME=/usr/local/Cellar/hadoop/3.0.0
export HADOOP_OPTS="--add-modules java.activation"  # Only for JDK 9+
```

2. Configure core-site.xml
   
```
<configuration>
  <property>
    <name>fs.defaultFS</name>
    <value>hdfs://localhost:9000</value>
  </property>
</configuration>
```

3. Configure hdfs-site.xml

This is a single-node HDFS installation (dfs.replication=1), meaning all HDFS components are running on your local machine rather than being distributed across multiple nodes, which is typical for development/testing environments. For production install, you can change replication to number of required nodes (dfs.replication=3) means that each block of data will be stored on 3 different DataNodes (slave nodes) for fault tolerance. 

```
<configuration>
  <property>
    <name>dfs.replication</name>
    <value>1</value>  <!-- Single-node cluster --> 
  </property>
  <property>
    <name>dfs.namenode.name.dir</name>
    <value>file://${HADOOP_HOME}/hadoop_data/namenode</value>
  </property>
  <property>
    <name>dfs.datanode.data.dir</name>
    <value>file://${HADOOP_HOME}/hadoop_data/datanode</value>
  </property>
  <property>
    <name>dfs.permissions</name>
    <value>false</value>  <!-- Disable strict permissions for dev -->
  </property>
</configuration>
```

4. Configure mapred-site.xml

```
<configuration>
  <property>
    <name>mapreduce.framework.name</name>
    <value>yarn</value>
  </property>
</configuration>

```
5. Configure yarn-site.xml

```
<configuration>
  <property>
    <name>yarn.nodemanager.aux-services</name>
    <value>mapreduce_shuffle</value>
  </property>
</configuration>
```

## Step 3: Initialize & Start Hadoop

Format HDFS (First-Time Setup)
```
hdfs namenode -format
```

Start Hadoop Services
```
start-dfs.sh    # Starts HDFS
start-yarn.sh   # Starts YARN
```

Verify Running Services
```
jps
```
Expected output:
```
NameNode
DataNode
SecondaryNameNode
ResourceManager
NodeManager
```

## Step 4: Access Hadoop Web UI

NameNode UI: http://localhost:9870
DataNode UI: http://localhost:9864

## Step 5: Basic HDFS Commands
```
hdfs dfs -mkdir /input            # Create directory
hdfs dfs -put localfile /input    # Upload file
hdfs dfs -ls /                    # List files
hdfs dfs -cat /input/file.txt     # View file
```

## Troubleshooting
1. Port Conflicts
Ensure ports 9000, 9870, and 9864 are free.
2. Permission Issues
```
sudo chown -R $USER ${HADOOP_HOME}/hadoop_data
```
3. Inconsistent FS State

- Clear temp data and reformat:
```
rm -rf /tmp/hadoop-*
hdfs namenode -format
```

**References**

# Big Data 
- Hadoop Official Docs - https://hadoop.apache.org/docs/stable/
- Michael Noll’s Tutorial - https://www.michael-noll.com/tutorials/running-hadoop-on-ubuntu-linux-single-node-cluster/
- https://www.michael-noll.com/tutorials/
  
Book: Big Data , Nathan Marz - Verlag: Manning
https://storm.apache.org/documentation/Tutorial.html
http://kafka.apache.org/
http://www.michael-noll.com/blog/2014/08/18/apache-kafka-training-deck-and-tutorial/ 
https://academy.datastax.com/courses/ds101-introduction-cassandra
https://academy.datastax.com/courses/ds201-cassandra-core-concepts
https://docs.docker.com/get-started/
https://dcos.io/docs/1.8/usage/tutorials/dcos-101/
