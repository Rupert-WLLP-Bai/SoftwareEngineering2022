import datetime
import json
import sqlite3

from flask import request, Blueprint

# 创建蓝图
user_api = Blueprint('user_api', __name__)


# 获取用户列表
@user_api.route('/api/user', methods=['GET'])
def user():
    # 不需要返回密码
    conn = sqlite3.connect('user.db')
    cursor = conn.cursor()
    sql = 'select id,name,email,phone,status,role,create_time from user'
    cursor.execute(sql)
    values = cursor.fetchall()
    cursor.close()
    conn.commit()
    conn.close()
    # 将数据转换为json格式
    res = {
        'success': True,
        'total': len(values),
        'data': []
    }
    for value in values:
        res['data'].append({
            'id': value[0],
            'name': value[1],
            'email': value[2],
            'phone': value[3],
            'status': value[4],
            'role': value[5],
            'create_time': value[6]
        })
    # 返回字典格式
    return res


# 增加用户
@user_api.route('/api/user', methods=['POST'])
def userAdd():
    # 获取参数
    data = request.get_data()
    print(data)
    # 如果参数为空
    if data == b'':
        return {
            'success': False,
            'message': '参数不能为空'
        }
    data = json.loads(data)
    print(data)
    id = data['id']
    name = data['name']
    password = '123456'
    email = data['email']
    phone = data['phone']
    status = data['status']
    role = data['role']
    create_time = datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    # 插入数据库
    conn = sqlite3.connect('user.db')
    cursor = conn.cursor()
    sql = 'insert into user (id,name,password,email,phone,status,role,create_time) values (?,?,?,?,?,?,?,?)'
    cursor.execute(sql, (id, name, password, email, phone, status, role,create_time))
    cursor.close()
    conn.commit()
    conn.close()
    # 返回json格式
    res = {
        'id': id,
        'name': name,
        'password': password,
        'email': email,
        'phone': phone,
        'status': status,
        'role': role
    }
    print(json.dumps(res))
    return {'success': True}


# 使用id删除用户
# DELETE /api/user?id=1 HTTP/1.1
@user_api.route('/api/user', methods=['DELETE'])
def userDelete():
    # 从url中获取id
    id = request.args.get('id')
    # 输出id
    print(id)
    # 从数据库删除
    conn = sqlite3.connect('user.db')
    cursor = conn.cursor()
    sql = "delete from user where id = '%s'" % id
    print("sql: ", sql)
    cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    # 返回json格式
    return {'success': True}

# 使用id修改用户
# PUT /api/user?id=1 HTTP/1.1
