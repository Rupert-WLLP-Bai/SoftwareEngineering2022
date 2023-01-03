#  公告 api
# title: 公告标题 varchar(255)
# description: 公告描述 varchar(255)
# createTime: 创建时间 timestamp
# publisher: 发布者 varchar(20)

import datetime
import json
import sqlite3

from flask import request, Blueprint

# 定义蓝图
notice_api = Blueprint('notice_api', __name__)


# 获取公告列表
@notice_api.route('/api/notice', methods=['GET'])
def notice():
    # 获取参数
    title = request.args.get('title')
    # 定义数据库连接
    conn = sqlite3.connect('notice.db')
    # 定义游标
    cursor = conn.cursor()
    # 如果没有参数则返回全部
    if title is None:
        # 查询数据条数，用total记录
        cursor.execute('select count(*) from notice')
        total = cursor.fetchone()[0]
        # 查询数据
        cursor.execute('select title, description, createTime, publisher from notice')
        values = cursor.fetchall()
        # 关闭游标
        cursor.close()
        # 提交事务
        conn.commit()
        # 关闭数据库连接
        conn.close()
        # 返回数据
        res = {
            'success': True,
            'total': total,
            'data': []
        }
        for value in values:
            res['data'].append({
                'title': value[0],
                'description': value[1],
                'createTime': value[2],
                'publisher': value[3]
            })
        # 返回字典格式
        return res
    # 如果有参数则返回符合条件的数据,返回一条记录
    else:
        # 查询数据
        cursor.execute('select title, description, createTime, publisher from notice where title=?', (title,))
        value = cursor.fetchone()
        # 关闭游标
        cursor.close()
        # 提交事务
        conn.commit()
        # 关闭数据库连接
        conn.close()
        # 返回数据
        # 如果没有查询到数据
        if value is None:
            res = {
                'success': False,
                'message': '没有查询到数据'
            }
            return res
        else:
            res = {
                'success': True,
                'title': value[0],
                'description': value[1],
                'createTime': value[2],
                'publisher': value[3]
            }
            return res


# 增加公告
@notice_api.route('/api/notice', methods=['POST'])
def noticeAdd():
    # 获取参数
    data = request.get_data()
    # 如果参数为空
    if data == b'':
        return {
            'success': False,
            'message': '参数不能为空'
        }
    data = json.loads(data)
    title = data['title']
    description = data['description']
    # createTime 使用时间戳
    createTime = datetime.datetime.now().timestamp() * 1000
    publisher = data['publisher']
    # 定义数据库连接
    conn = sqlite3.connect('notice.db')
    # 定义游标
    cursor = conn.cursor()
    # 插入数据
    cursor.execute('insert into notice (title, description, createTime, publisher) values (?,?,?,?)',
                   (title, description, createTime, publisher))
    # 关闭游标
    cursor.close()
    # 提交事务
    conn.commit()
    # 关闭数据库连接
    conn.close()
    # 返回数据
    return {
        'success': True,
        'message': '添加成功'
    }


# 按title删除公告
@notice_api.route('/api/notice', methods=['DELETE'])
def noticeDelete():
    # 获取参数
    title = request.args.get('title')
    # 如果参数为空
    if title is None:
        return {
            'success': False,
            'message': '参数不能为空'
        }
    # 定义数据库连接
    conn = sqlite3.connect('notice.db')
    # 定义游标
    cursor = conn.cursor()
    # 删除数据
    cursor.execute('delete from notice where title=?', (title,))
    # 关闭游标
    cursor.close()
    # 提交事务
    conn.commit()
    # 关闭数据库连接
    conn.close()
    # 返回数据
    return {
        'success': True,
        'message': '删除成功'
    }


# 按title修改公告
@notice_api.route('/api/notice', methods=['PUT'])
def noticeUpdate():
    # 获取参数
    data = request.get_data()
    # 如果参数为空
    if data == b'':
        return {
            'success': False,
            'message': '参数不能为空'
        }
    data = json.loads(data)
    title = data['title']
    description = data['description']
    publisher = data['publisher']
    # 定义数据库连接
    conn = sqlite3.connect('notice.db')
    # 定义游标
    cursor = conn.cursor()
    # 修改数据
    cursor.execute('update notice set description=?, publisher=? where title=?', (description, publisher, title))
    # 关闭游标
    cursor.close()
    # 提交事务
    conn.commit()
    # 关闭数据库连接
    conn.close()
    # 返回数据
    return {
        'success': True,
        'message': '修改成功'
    }
