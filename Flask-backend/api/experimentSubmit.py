# 学生提交实验
import sqlite3
from io import BytesIO

from flask import request, send_file
from flask import Blueprint

# Path: api\experimentSubmit.py
# 创建蓝图
experimentSubmit_api = Blueprint('experimentSubmit_api', __name__)


@experimentSubmit_api.route('/api/experiment/submit', methods=['POST'])
def submit():
    # 获取表单数据
    form = request.form
    # 获取文件
    f = request.files['file']
    # 存储到experimentSubmit.db
    # id: 学生id varchar(10)
    # title: 实验名称 varchar(255)
    # submitTime: 提交时间 timestamp
    # file: 实验文件 blob
    conn = sqlite3.connect('experimentSubmit.db')
    cursor = conn.cursor()
    cursor.execute('insert into experimentSubmit values (?,?,?,?,?)',
                   (form['id'], form['title'], form['timeStamp'], f.read(), f.filename))
    conn.commit()
    cursor.close()
    conn.close()
    # 读取数据库输出条目数
    # conn = sqlite3.connect('experimentSubmit.db')
    # cursor = conn.cursor()
    # cursor.execute('select * from experimentSubmit')
    # values = cursor.fetchall()
    # print("实验提交数: ", len(values))
    return {'success': True}


# 获取学生提交的实验信息
@experimentSubmit_api.route('/api/experiment/submit', methods=['GET'])
def getSubmit():
    # 读取数据库
    conn = sqlite3.connect('experimentSubmit.db')
    cursor = conn.cursor()
    cursor.execute('select * from experimentSubmit')
    values = cursor.fetchall()
    print("实验提交数: ", len(values))
    # 返回json
    res = {
        'success': True,
        'total': len(values),
        'data': []
    }
    for value in values:
        res['data'].append({
            'id': value[0],
            'title': value[1],
            'submitTime': value[2],
        })
    print(res)
    return res


# 获取学生提交的实验文件
# 使用id,title,submitTime确定唯一的实验文件
@experimentSubmit_api.route('/api/experiment/submit/file', methods=['GET'])
def getSubmitFile():
    # 获取参数
    id = request.args.get('id')
    title = request.args.get('title')
    submitTime = request.args.get('submitTime')
    # 读取数据库
    conn = sqlite3.connect('experimentSubmit.db')
    cursor = conn.cursor()
    cursor.execute('select file, filename from experimentSubmit where id=? and title=? and submitTime=?',
                   (id, title, submitTime))
    value = cursor.fetchone()
    # 返回文件
    # 文件名是value[1]
    filename = value[1]
    # 文件内容是value[0]
    file = value[0]
    # 使用BytesIO将文件内容转换为文件对象
    file = BytesIO(file)
    # 使用send_file发送文件,设置download_name为文件名,浏览器弹出下载框
    return send_file(file, as_attachment=True, download_name=filename)


# 获取学生第一次提交的时间和最后一次提交的时间
@experimentSubmit_api.route('/api/experiment/submit/student', methods=['GET'])
def getSubmitTime():
    # 获取参数
    title = request.args.get('title')
    print(title)
    # 读取数据库
    conn = sqlite3.connect('experimentSubmit.db')
    cursor = conn.cursor()
    cursor.execute('select min(submitTime), max(submitTime) from experimentSubmit where title=?',
                   (title,))
    value = cursor.fetchone()
    firstSubmitTime = value[0]
    lastSubmitTime = value[1]
    print(firstSubmitTime, lastSubmitTime)
    cursor.close()
    conn.close()
    # 用title查询实验信息
    conn = sqlite3.connect('experiment.db')
    cursor = conn.cursor()
    cursor.execute('select startTime,endTime,status from experiment where title=?', (title,))
    value = cursor.fetchone()
    startTime = value[0]
    endTime = value[1]
    status = value[2]
    cursor.close()
    conn.close()
    # 返回json
    res = {
        'success': True,
        'data': {
            'firstSubmitTime': firstSubmitTime,
            'lastSubmitTime': lastSubmitTime,
            'startTime': startTime,
            'endTime': endTime,
            'status': status
        }
    }
    return res



