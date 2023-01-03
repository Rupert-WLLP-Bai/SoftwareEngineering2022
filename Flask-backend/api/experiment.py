import sqlite3
import urllib

from flask import request, Blueprint

# 创建蓝图
experiment_api = Blueprint('experiment_api', __name__)


# curl "http://124.223.95.200/api/experiment/1/1"
# @app.route('/api/experiment/<current>/<pageSize>',methods=['GET'])
@experiment_api.route('/api/experiment', methods=['GET'])
def experiment():
    # 获取参数
    title = request.args.get('title')
    # 定义数据库连接
    conn = sqlite3.connect('experiment.db')
    # 定义游标
    cursor = conn.cursor()
    # 如果没有参数则返回全部
    if title is None:
        # 查询数据条数，用total记录
        cursor.execute('select count(*) from experiment')
        total = cursor.fetchone()[0]
        # 查询数据
        cursor.execute('select title, startTime, endTime, status, description, publishDate from experiment')
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
                'startTime': value[1],
                'endTime': value[2],
                'status': value[3],
                'description': value[4],
                'publishDate': value[5]
            })
        # 返回字典格式
        return res
    # 如果有参数则返回符合条件的数据,返回一条记录
    else:
        # 查询数据
        cursor.execute(
            'select title, startTime, endTime, status, description, publishDate from experiment where title=?',
            (title,))
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
                'startTime': value[1],
                'endTime': value[2],
                'status': value[3],
                'description': value[4],
                'publishDate': value[5]
            }
            # 返回字典格式
            return res


@experiment_api.route('/api/experiment', methods=['POST'])
def addExperiment():
    print(request.data.decode('utf-8'))
    return 'ok'


# 删除实验
# curl  -X "DELETE" --data-urlencode "title=q" 150.158.80.33:5000/api/experiment
# DELETE /api/experiment?title=q HTTP/1.1
@experiment_api.route('/api/experiment', methods=['DELETE'])
def deleteExperiment():
    # 从url中获取title
    title = request.form['title']
    # 将中文转换为utf-8
    title = urllib.parse.unquote(title)
    # 输出title
    print(title)
    # 从数据库删除
    conn = sqlite3.connect('experiment.db')
    cursor = conn.cursor()
    sql = "delete from experiment where title = '%s'" % title
    print("sql: ", sql)
    cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    # 读取数据库输出条目数
    conn = sqlite3.connect('experiment.db')
    cursor = conn.cursor()
    cursor.execute('select * from experiment')
    values = cursor.fetchall()
    print("实验提交数: ", len(values))
    return {'success': True}


# 发布实验
@experiment_api.route('/api/experiment/publish', methods=['POST'])
def upload_experiment_publish():
    f = request.files['file']
    form = request.form.to_dict()
    print(form)
    print(f)
    # TODO: 存到数据库 f.save('/home/ubuntu/Simple-api/static' + '/' +str(params['id'] + "_" + params['time'] + "_" +
    #  params['type'] + "_" + params['filename'])) 存储到experiment.db
    conn = sqlite3.connect('experiment.db')
    cursor = conn.cursor()
    cursor.execute(
        'insert into experiment (title, description, publishDate, startTime, endTime, file) values (?,?,?,?,?,?)',
        (form['title'], form['description'], form['publishDate'], form['startTime'], form['endTime'], f.read()))
    cursor.close()
    conn.commit()
    conn.close()
    # 从数据库读取并输出
    conn = sqlite3.connect('experiment.db')
    cursor = conn.cursor()
    cursor.execute('select * from experiment')
    values = cursor.fetchall()
    print(values)
    return {'success': True}
