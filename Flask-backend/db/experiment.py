# experiment 建表
# id: 实验id int auto_increment
# title: 实验名称 varchar(255)
# description: 实验描述 varchar(255)
# publishDate: 发布日期 timestamp
# startTime: 开始时间 timestamp
# endTime: 结束时间 timestamp
# status: 状态 int
# file: 实验文件 blob
import sqlite3


def init_db_experiment():
    conn = sqlite3.connect('experiment.db')
    cursor = conn.cursor()
    cursor.execute(
        'create table experiment (id integer primary key autoincrement, title varchar(255), description varchar(255), publishDate timestamp, startTime timestamp, endTime timestamp, status int, file blob)')
    cursor.close()
    conn.commit()
    conn.close()
    print("experiment.db init success")
