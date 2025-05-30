🛠️ Enabling Kerberos in Hadoop: Step-by-Step

Step 1: 🧱 Set up a KDC
On RHEL/CentOS:
```
sudo yum install krb5-server krb5-libs krb5-workstation
sudo systemctl enable krb5kdc
sudo systemctl start krb5kdc
```

Step 2: 🗃️ Configure /etc/krb5.conf
```
[libdefaults]
  default_realm = HADOOP.COM
  dns_lookup_kdc = false

[realms]
  HADOOP.COM = {
    kdc = kdc.hadoop.com
    admin_server = kdc.hadoop.com
  }

[domain_realm]
  .hadoop.com = HADOOP.COM
  hadoop.com = HADOOP.COM
```
Step 3: 🔐 Create Principals
```
kadmin.local
addprinc hdfs/hadoop1.hadoop.com@HADOOP.COM
addprinc yarn/hadoop2.hadoop.com@HADOOP.COM
addprinc user1@HADOOP.COM
```
Step 4: 💾 Generate Keytab Files
```
ktadd -k hdfs.service.keytab hdfs/hadoop1.hadoop.com@HADOOP.COM
ktadd -k user1.keytab user1@HADOOP.COM
Place these securely on respective machines.
```
Step 5: ⚙️ Configure Hadoop for Kerberos

hdfs-site.xml
```
<property>
  <name>dfs.namenode.kerberos.principal</name>
  <value>hdfs/_HOST@HADOOP.COM</value>
</property>
<property>
  <name>dfs.namenode.keytab.file</name>
  <value>/etc/security/keytabs/hdfs.service.keytab</value>
</property>
```
core-site.xml
```
<property>
  <name>hadoop.security.authentication</name>
  <value>kerberos</value>
</property>
<property>
  <name>hadoop.security.authorization</name>
  <value>true</value>
</property>
```
* Apply similar configuration for yarn-site.xml, mapred-site.xml, hive-site.xml, etc. * 

Step 6: ✅ Test Authentication
```
kinit -kt user1.keytab user1@HADOOP.COM
hdfs dfs -ls /user/user1
```
If successful, you're authenticated!

📋 Use Cases in Hadoop Ecosystem
```
Component	How Kerberos is Used
HDFS	Secure access to data blocks
YARN	Authentication of NodeManagers and ResourceManager
Hive	Secure metadata access via HiveServer2
Spark	Secure job submission and service-to-service auth
HBase	Authenticated read/write from/to tables
Kafka	Broker and client authentication
```
✅ Best Practices
```
Rotate keytabs and renew tickets frequently
Secure keytab files with permissions
Use SPNEGO (Simple and Protected GSSAPI Negotiation Mechanism) for HTTP endpoints like WebHDFS
Integrate with LDAP for identity management
Automate renewal of kinit using cron or ticket renewal scripts
```
🔧 Code Example: Hadoop Job with Kerberos Auth
```
export HADOOP_USER_NAME=user1
kinit -kt /etc/security/keytabs/user1.keytab user1@HADOOP.COM

hadoop jar my-job.jar com.example.MyMapReduce \
  -D mapreduce.job.queuename=securequeue \
  /input/path /output/path
```
🔧 Code Example: Spark Job with Kerberos Auth
```
spark-submit \
  --master yarn \
  --deploy-mode cluster \
  --principal sparkuser@YOUR.REALM.COM \
  --keytab /path/to/user.keytab \
  --conf "spark.hadoop.security.authentication=kerberos" \
  --conf "spark.hadoop.security.authorization=true" \
  my_spark_job.py
```

🧩 Visual Overview

    +-----------+         +----------------+         +-----------+
    |  USER     |  --->   |   KDC Server   |  --->   |  TGT      |
    +-----------+         +----------------+         +-----------+
         |                          |                       |
         | ---- Service Ticket Request -------------------->|
         |<--- Service Ticket Issued -----------------------|
         |                          |                       |
         | ---- Present Service Ticket to HDFS ------------>|
         |<--- Access Granted if Ticket Valid --------------|
