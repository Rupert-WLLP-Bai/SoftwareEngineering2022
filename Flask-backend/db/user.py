# user 建表
# id: 学生id varchar(10)
# name: 学生姓名 varchar(20)
# password: 学生密码 varchar(20)
# email: 学生邮箱 varchar(255)
# phone: 学生电话 varchar(20)
# status: 学生状态 int
# role: 学生角色 int
# create_time: 创建时间 timestamp
import random
import sqlite3
import datetime

def init_db_user():
    conn = sqlite3.connect('user.db')
    cursor = conn.cursor()
    cursor.execute(
        'create table user (id varchar(10), name varchar(20), password varchar(20), email varchar(255), '
        'phone varchar(20), status int, role int , create_time timestamp)')
    cursor.close()
    conn.commit()
    conn.close()
    print("user.db init success")


# 初始用户数据
def init_user():
    # id是 20xxxxx 随机生成
    # name是 学生姓名 随机
    # password 123456
    # email是 学生邮箱 随机生成
    # phone是 学生电话 随机11位数字
    # status是 学生状态 0-正常 1-禁用
    # role是 学生角色 0-管理员 1-学生 2-教师 3-助教
    # create_time是 创建时间
    # 随机生成信息
    def random_user():
        user_id = random_id()
        email = user_id + '@tongji.edu.cn'
        return {
            'id': user_id,
            'name': random_name(),
            'password': '123456',
            'email': email,
            'phone': random_phone(),
            'status': random_status(),
            'role': random_role(),
            # create_time是 创建时间 timestamp
            'create_time': datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        }

    def random_id():
        # 2000000 ~ 2099999
        return str(random.randint(2000000, 2099999))

    def random_name():
        # random chinese first name dictionary
        # random chinese last name dictionary
        chinese_first_name = ["赵", "钱", "孙", "李", "周", "吴", "郑", "王", "冯", "陈", "褚", "卫", "蒋", "沈", "韩",
                              "杨", "朱", "秦", "尤", "许", "何", "吕", "施", "张", "孔", "曹", "严", "华", "金", "魏",
                              "陶", "姜", "戚", "谢", "邹", "喻", "柏", "水", "窦", "章", "云", "苏", "潘", "葛", "奚",
                              "范", "彭", "郎", "鲁", "韦", "昌", "马", "苗", "凤", "花", "方", "俞", "任", "袁", "柳",
                              "酆", "鲍", "史", "唐", "费", "廉", "岑", "薛", "雷", "贺", "倪", "汤", "滕", "殷", "罗",
                              "毕", "郝", "邬", "安", "常", "乐", "于", "时", "傅", "皮", "卞", "齐", "康", "伍", "余",
                              "元", "卜", "顾", "孟", "平", "黄", "和", "穆", "萧", "尹", "姚", "邵", "湛", "汪", "祁",
                              "毛", "禹", "狄", "米", "贝", "明", "臧", "计", "伏", "成", "戴", "谈", "宋", "茅", "庞",
                              "熊", "纪", "舒", "屈", "项", "祝", "董"]
        chinese_last_name = ["伟", "芳", "娜", "秀英", "敏", "静", "丽", "强", "磊", "军", "洋", "勇", "艳", "杰", "涛",
                             "明", "超", "刚", "平", "辉", "霞", "秀兰", "桂英"]
        # random chinese name
        return random.choice(chinese_first_name) + random.choice(chinese_last_name)

    def random_phone():
        # random phone
        return str(random.randint(10000000000, 19999999999))

    def random_status():
        # random status
        return random.randint(0, 1)

    def random_role():
        # random role
        return random.randint(0, 3)

    # 生成50个用户
    users = [random_user() for i in range(50)]
    # 插入数据库
    conn = sqlite3.connect('user.db')
    cursor = conn.cursor()
    for user in users:
        sql_1 = 'insert into user (id, name, password, email, phone, status, role, create_time)'
        sql_2 = 'values (?,?,?,?,?,?,?,?)'
        sql = sql_1 + sql_2
        cursor.execute(sql, (user['id'], user['name'], user['password'], user['email'], user['phone'],
                                user['status'],user['role'],user['create_time']))
    cursor.close()
    conn.commit()
    conn.close()
