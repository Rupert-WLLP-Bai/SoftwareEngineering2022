import datetime
import json
import os
import sqlite3
import time

from flask import Flask, request, make_response, send_from_directory
from flask_cors import CORS

from api.experiment import experiment_api
from api.experimentSubmit import experimentSubmit_api
from api.notice import notice_api
from api.user import user_api
from db.experiment import init_db_experiment
from db.experimentSubmit import init_db_experimentSubmit
from db.notice import init_db_notice
from db.user import init_db_user, init_user

app = Flask(__name__)
app.config['JSON_AS_ASCII'] = False
app.register_blueprint(user_api)
app.register_blueprint(experiment_api)
app.register_blueprint(experimentSubmit_api)
app.register_blueprint(notice_api)
CORS(app)

# 不存在则创建数据库
if not os.path.exists('experiment.db'):
    init_db_experiment()

# 不存在则创建数据库
if not os.path.exists('experimentSubmit.db'):
    init_db_experimentSubmit()

# 不存在则创建数据库
if not os.path.exists('user.db'):
    init_db_user()
    # 初始化用户数据
    init_user()

if not os.path.exists('notice.db'):
    init_db_notice()

'''
实验相关 文件的上传和下载
by cn 2023-1-1 01:11 AM
'''


# curl -X "POST" "http://124.223.95.200/api/upload?id=123&type=experiment"  -F "files=@1.txt"
# curl -X "POST" "http://124.223.95.200/api/upload?id=123&type=experimentSubmit"  -F "files=@1.txt"
@app.route('/api/upload/experiment/submit', methods=['POST'])
def upload_experiment_submit():
    f = request.files['files']

    params = {
        'id': request.args.get('id'),
        'experimentId': '1',
        'time': str(int(time.time())),
        'filename': f.filename
    }

    f.save('/home/ubuntu/Simple-api/static' + '/' + str(
        params['id'] + "_" + params['time'] + "_" + params['type'] + "_" + params['filename']))
    return str(params['id'] + "_" + params['time'] + "_" + params['type'] + "_" + params['filename'])


# 123_2023-01-01004903_experiment_1.txt
# curl "http://124.223.95.200/api/download/123_2023-01-01010436_experiment_API.zip" 
@app.route('/api/download/<filename>', methods=['GET'])
def download_file(filename):
    # 文件路径在"./file"目录下
    # 将当前目录下的文件名为filename的文件发送给客户端
    # 获取当前目录的绝对路径
    directory = os.path.abspath(os.path.dirname(__file__))
    path = os.path.join(directory, 'file', filename)
    return send_from_directory('file', filename, as_attachment=True)


# 上传文件
@app.route('/api/upload', methods=['POST'])
def upload_file():
    # 文件路径在"./file"目录下
    # 文件名是用户上传的文件名
    # 获取当前目录的绝对路径
    directory = os.path.abspath(os.path.dirname(__file__))
    path = os.path.join(directory, 'file')
    # 获取上传的文件
    f = request.files['files']
    # 保存文件
    f.save(os.path.join(path, f.filename))
    return f.filename


@app.route('/api/currentUser', methods=['GET'])
def currentUser():
    # 返回值格式参考
    # res = {
    #     "success": True,
    #     "data": {
    #         "id": "2052538",
    #         "name": "Chennuo",
    #         "email": "jormongand@gmail.com",
    #         "phone": "19821378960",
    #         "role": 0,
    #         "status": 1,
    #         "access": 'admin',
    #         "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0ZTllMDYwNS0xMTNkLTRmMjQtODc3OC0zZGY0Y2EzNTBmOTgiLCJpZCI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJpc3N1ZWRBdCI6IjIwMjItMTEtMzBUMTg6MjY6MjAiLCJuYmYiOjE2Njk4MDM5ODAsImV4cCI6MTY2OTgwNDA0MCwiaXNzIjoiU2ltcGxlT2ouYWRtaW4iLCJhdWQiOiJhZG1pbiJ9.-CgHSkvEzd6LQ_XEppqHlyn0-9646vMBvMDQFdAz-cs",
    #         "ip": "111.187.106.17"
    #     }
    # }

    # 从请求的authorization中获取id
    id = request.headers.get('authorization')
    print("[currentUser] id: " + id)
    # 从数据库中查询id对应的用户信息
    conn = sqlite3.connect('user.db')
    c = conn.cursor()
    c.execute("SELECT * FROM user WHERE id = ?", (id,))
    user = c.fetchone()
    conn.close()
    print(user)
    # 从请求中获取ip
    # TODO 解决返回的ip一直是127.0.0.1
    if request.environ.get('HTTP_X_FORWARDED_FOR') is None:
        print(request.environ['REMOTE_ADDR'])
    else:
        print(request.environ['HTTP_X_FORWARDED_FOR'])

    ip = request.headers.get('X-Real-IP', request.remote_addr)
    print("[currentUser] ip: " + ip)
    # 返回res
    # access 0:admin 1:school 2:teacher 3:assistant
    res = {
        "success": True,
        "data": {
            "id": user[0],
            "name": user[1],
            "password": user[2],
            "email": user[3],
            "phone": user[4],
            "status": user[5],
            "role": user[6],
            "createTime": user[7],
            "ip": ip,
            "access": "admin",
            "token": id
        }
    }
    print("currentUser: ", res)
    return res


@app.route('/api/login/outLogin', methods=['GET'])
def outLogin():
    authorization = request.headers.get('authorization')
    return authorization


@app.route('/api/login/account', methods=['POST'])
def login():
    login_json = json.loads(request.get_data())
    id = login_json['id']
    password = login_json['password']
    # 读取数据库
    conn = sqlite3.connect('user.db')
    c = conn.cursor()
    c.execute("SELECT * FROM user WHERE id = ?", (id,))
    user = c.fetchone()
    conn.close()
    if user is None:
        return {
            "success": False,
            "message": "用户名不存在"
        }
    if user[2] != password:
        return {
            "success": False,
            "message": "密码错误"
        }
    return {
        "success": True,
        "data": {
            "currentAuthority": "user",
            "token": id
        }
    }


# 用户注册
@app.route('/api/register', methods=['POST'])
def register():
    # 请求是json提交
    register_json = json.loads(request.get_data())
    id = register_json['id']
    password = register_json['password']
    name = register_json['name']
    email = register_json['email']
    phone = register_json['phone']
    status = 1
    role = 1
    createTime = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    print(id, password, name, email, phone, type(status), role, createTime)
    # 读取数据库
    conn = sqlite3.connect('user.db')
    c = conn.cursor()
    c.execute("SELECT * FROM user WHERE id = ?", (id,))
    user = c.fetchone()
    if user is not None:
        return {
            "success": False,
            "message": "用户名已存在"
        }
    c.execute("INSERT INTO user VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
              (id, name, password, email, phone, status, role, createTime))
    conn.commit()
    conn.close()
    return {
        "success": True,
        "data": {
            "id": id,
            "name": name,
            "password": password,
            "email": email,
            "phone": phone,
            "status": status,
            "role": role,
            "createTime": createTime
        }
    }
