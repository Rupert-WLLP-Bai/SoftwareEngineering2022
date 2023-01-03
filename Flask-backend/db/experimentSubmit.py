# experimentSubmit 建表
# id: 学生id varchar(10)
# title: 实验名称 varchar(255)
# submitTime: 提交时间 timestamp
# file: 实验文件 blob
# fileName: 实验文件名 varchar(255)
import sqlite3


def init_db_experimentSubmit():
    conn = sqlite3.connect('experimentSubmit.db')
    cursor = conn.cursor()
    cursor.execute(
        'create table experimentSubmit (id varchar(10), title varchar(255), submitTime timestamp, file blob, '
        'fileName varchar(255))')
    cursor.close()
    conn.commit()
    conn.close()
    print("experimentSubmit.db init success")
