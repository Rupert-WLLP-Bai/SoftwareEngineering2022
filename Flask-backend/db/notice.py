# 公告 建表
# title: 公告标题 varchar(255)
# description: 公告描述 varchar(255)
# createTime: 创建时间 timestamp
# publisher: 发布者 varchar(20)

import sqlite3

def init_db_notice():
    conn = sqlite3.connect('notice.db')
    cursor = conn.cursor()
    cursor.execute(
        'create table notice (title varchar(255), description varchar(255), createTime timestamp, publisher varchar(20))')
    cursor.close()
    conn.commit()
    conn.close()
    print("notice.db init success")

