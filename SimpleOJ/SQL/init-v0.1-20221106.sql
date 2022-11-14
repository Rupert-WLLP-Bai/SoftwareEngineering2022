-- Active: 1667725277146@@119.3.154.46@3306@SE2022

-- 软件工程课程设计

-- 2021-11-06 17:42:00

-- 初始数据

-- 1. User表

-- 1.1 管理员

/*
 用户名：admin 密码：admin 角色：管理员
 */

INSERT INTO `user`
VALUES (
        'admin',
        'admin',
        'admin',
        'admin@gmail.com',
        '178xxxyyyy',
        0,
        1,
        CURRENT_TIMESTAMP(),
        CURRENT_TIMESTAMP()
    );

-- 1.2 普通用户

/*
 用户名：student1 密码：student1 角色：学生
 用户名：student2 密码：student2 角色：学生
 用户名：student3 密码：student3 角色：学生
 用户名：teacher1 密码：teacher1 角色：教师
 用户名：teacher2 密码：teacher2 角色：教师
 用户名：ta1 密码：ta1 角色：助教
 用户名：ta2 密码：ta2 角色：助教
 */

INSERT INTO `user`
VALUES (
        '2050001',
        'student1',
        'student1',
        'student1@gmail.com',
        '178xxxyyyy',
        1,
        1,
        CURRENT_TIMESTAMP(),
        CURRENT_TIMESTAMP()
    ), (
        '2050002',
        'student2',
        'student2',
        'student2@gmail.com',
        '178xxxyyyy',
        1,
        1,
        CURRENT_TIMESTAMP(),
        CURRENT_TIMESTAMP()
    ), (
        '2050003',
        'student3',
        'student3',
        'student3@gmail.com',
        '178xxxyyyy',
        1,
        1,
        CURRENT_TIMESTAMP(),
        CURRENT_TIMESTAMP()
    ), (
        '01001',
        'teacher1',
        'teacher1',
        'teacher1@gmail.com',
        '178xxxyyyy',
        2,
        1,
        CURRENT_TIMESTAMP(),
        CURRENT_TIMESTAMP()
    ), (
        '02002',
        'teacher2',
        'teacher2',
        'teacher2@gmail.com)',
        '178xxxyyyy',
        2,
        1,
        CURRENT_TIMESTAMP(),
        CURRENT_TIMESTAMP()
    ), (
        '2010001',
        'ta1',
        'ta1',
        'ta1@gmail.com',
        '178xxxyyyy',
        3,
        1,
        CURRENT_TIMESTAMP(),
        CURRENT_TIMESTAMP()
    ), (
        '2010002',
        'ta2',
        'ta2',
        'ta2@gmail.com',
        '178xxxyyyy',
        3,
        1,
        CURRENT_TIMESTAMP(),
        CURRENT_TIMESTAMP()
    );

--