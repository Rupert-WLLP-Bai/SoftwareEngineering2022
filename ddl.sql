-- Active: 1667700485300@@119.3.154.46@3306@SE2022

/*
    2022/11/07 Update
        1. 密码部分加入盐值，使用SHA256加密(更改了User表的字段，加入了salt字段，将password字段改为64位)
        2. 初始数据部分在.NET Core代码中实现
*/

/* 软件工程课程设计DDL */

/* 1.创建模式 */

DROP SCHEMA IF EXISTS `SE2022`;

CREATE SCHEMA IF NOT EXISTS `SE2022` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

USE `SE2022`;

/* 2.创建表 */

/* 2.1 user表 */

DROP TABLE IF EXISTS `user`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`user`(
        `id` VARCHAR(36) NOT NULL COMMENT '用户id,学工号',
        -- 用户id,学工号
        `password` VARCHAR(64) NOT NULL COMMENT '密码 加盐 SHA256加密',
        -- 密码 加盐 SHA256加密 
        `salt` VARCHAR(36) NOT NULL COMMENT '盐值',
        -- 盐值
        `name` VARCHAR(255) NOT NULL COMMENT '姓名',
        -- 姓名
        `email` VARCHAR(255) NOT NULL COMMENT '邮箱',
        -- 邮箱
        `phone` VARCHAR(255) NOT NULL COMMENT '电话',
        -- 电话
        `role` INT NOT NULL COMMENT '角色,0为管理员,1为学生,2为教师,3为助教',
        -- 角色,0为管理员,1为学生,2为教师,3为助教
        `status` INT NOT NULL COMMENT '用户状态,0为禁用,1为正常',
        -- 用户状态,0为禁用,1为正常
        `create_time` DATETIME NOT NULL COMMENT '用户创建时间',
        -- 用户创建时间
        `update_time` DATETIME NOT NULL COMMENT '用户更新时间',
        -- 用户更新时间
        PRIMARY KEY (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '用户表';

/* 2.2 experiment表 */

DROP TABLE IF EXISTS `experiment`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`experiment` (
        `id` VARCHAR(36) NOT NULL COMMENT '实验id,uuid()',
        -- 实验id，uuid()
        `teacher_id` VARCHAR(36) NOT NULL COMMENT '教师id',
        -- 教师id
        `name` VARCHAR(255) NOT NULL COMMENT '实验名称',
        -- 实验名称
        `description` TEXT NOT NULL COMMENT '实验描述',
        -- 实验描述
        `file_path` VARCHAR(255) COMMENT '实验文件路径,没有文件则为null',
        -- 实验文件路径,没有文件则为null
        `status` INT NOT NULL COMMENT '实验状态,0为未发布,1为已发布,2为已结束',
        -- 实验状态,0为未发布,1为已发布,2为已结束
        `create_time` DATETIME NOT NULL COMMENT '实验创建时间',
        -- 实验创建时间
        `update_time` DATETIME NOT NULL COMMENT '实验更新时间',
        -- 实验更新时间
        `distribute_time` DATETIME COMMENT '实验发布时间,未发布则为空',
        -- 实验发布时间,未发布则为空
        `start_time` DATETIME COMMENT '实验开始时间,未发布则为空',
        -- 实验开始时间,未发布则为空
        `end_time` DATETIME COMMENT '实验截止时间,未发布则为空',
        -- 实验截止时间,未发布则为空
        `upload_times_limit` INT COMMENT '实验提交次数限制,未发布则为空',
        -- 实验提交次数限制,未发布则为空
        PRIMARY KEY (`id`),
        FOREIGN KEY (`teacher_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '实验表';

/* 2.3 total_score表 */

DROP TABLE IF EXISTS `total_score`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`total_score`(
        `id` VARCHAR(36) NOT NULL COMMENT '成绩id,uuid()',
        -- 成绩id，uuid()
        `student_id` VARCHAR(36) NOT NULL COMMENT '学生id',
        -- 学生id
        `score` INT NOT NULL DEFAULT 0 COMMENT '成绩',
        -- 成绩
        `create_time` DATETIME NOT NULL COMMENT '成绩创建时间',
        -- 成绩创建时间
        `update_time` DATETIME NOT NULL COMMENT '成绩更新时间',
        -- 成绩更新时间
        PRIMARY KEY (`id`),
        FOREIGN KEY (`student_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '总成绩表';

/* 2.4 experiment_handin表 */

DROP TABLE IF EXISTS `experiment_handin`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`experiment_handin`(
        `id` VARCHAR(36) NOT NULL COMMENT '实验提交id,uuid()',
        -- 实验提交id，uuid()
        `experiment_id` VARCHAR(36) NOT NULL COMMENT '实验id',
        -- 实验id
        `student_id` VARCHAR(36) NOT NULL COMMENT '学生id',
        -- 学生id
        `file_path` VARCHAR(255) NOT NULL COMMENT '实验提交文件路径',
        -- 实验提交文件路径
        `create_time` DATETIME NOT NULL COMMENT '实验提交创建时间',
        -- 实验提交创建时间
        `update_time` DATETIME NOT NULL COMMENT '实验提交更新时间',
        -- 实验提交更新时间
        PRIMARY KEY (`id`),
        FOREIGN KEY (`experiment_id`) REFERENCES `experiment` (`id`),
        FOREIGN KEY (`student_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '实验提交表';

/* 2.5 problem表 */

DROP TABLE IF EXISTS `problem`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`problem`(
        `id` VARCHAR(36) NOT NULL COMMENT '题目id,uuid()',
        -- 题目id,uuid()
        `title` VARCHAR(255) NOT NULL COMMENT '题目标题',
        -- 题目标题
        `description` TEXT NOT NULL COMMENT '题目描述',
        -- 题目描述
        `tag` VARCHAR(255) COMMENT '题目标签',
        -- 题目标签
        `sample_input` TEXT NOT NULL COMMENT '题目样例输入',
        -- 题目样例输入
        `sample_output` TEXT NOT NULL COMMENT '题目样例输出',
        -- 题目样例输出
        `sample_explaination` TEXT NOT NULL COMMENT '题目样例解释',
        -- 题目样例解释
        /* 
         以上三个字段使用Json分别以数组形式存储，样例如下:
         // input
         {
         "input": [
         "1,2,3,4,5",
         "1,2,3,4,5"
         ]
         }
         // output
         {
         "output": [
         "1,2,3,4,5",
         "5,4,3,2,1"
         ]
         }
         // explaination
         {
         "explaination": [
         "The input and the output are the same.",
         "The input and the output are reversed."
         ]
         }
         
         // 前端显示
         [Example 1]
         Input: [1,2,3,4,5]
         Output: [1,2,3,4,5]
         Explanation: The input and the output are the same.
         [Example 2]
         Input: [5,4,3,2,1]
         Output: [1,2,3,4,5]
         Explanation: The input is reversed.
         */
        `difficulty` INT NOT NULL COMMENT '题目难度,0为Easy,1为Medium,2为Hard',
        -- 题目难度,0为Easy,1为Medium,2为Hard
        `create_time` DATETIME NOT NULL COMMENT '题目创建时间',
        -- 题目创建时间
        `update_time` DATETIME NOT NULL COMMENT '题目更新时间',
        -- 题目更新时间
        `code_template` VARCHAR(255) NOT NULL COMMENT '题目代码模板',
        -- 题目代码模板
        /*
         class Solution {
         public:
         vector<int> twoSum(vector<int>& nums, int target) {
         
         }
         };
         */
        `test_case` TEXT NOT NULL COMMENT '题目测试用例',
        -- 题目测试用例
        PRIMARY KEY (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '题目表';

/* 2.6 examination表 */

DROP TABLE IF EXISTS `examination`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`examination`(
        `id` VARCHAR(36) NOT NULL COMMENT '测验id,uuid()',
        -- 测验id,uuid()
        `title` VARCHAR(255) NOT NULL COMMENT '测验标题',
        -- 测验标题
        `description` TEXT NOT NULL COMMENT '测验描述',
        -- 测验描述
        `start_time` DATETIME NOT NULL COMMENT '测验开始时间',
        -- 测验开始时间
        `end_time` DATETIME NOT NULL COMMENT '测验结束时间',
        -- 测验结束时间
        `create_time` DATETIME NOT NULL COMMENT '测验创建时间',
        -- 测验创建时间
        `update_time` DATETIME NOT NULL COMMENT '测验更新时间',
        -- 测验更新时间
        `distributed_time` DATETIME NOT NULL COMMENT '测验发布时间',
        -- 测验发布时间
        `status` INT NOT NULL COMMENT '测验状态,0为未发布,1为已发布,2为已结束',
        -- 测验状态,0为未发布,1为已发布,2为已结束
        PRIMARY KEY (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '测验表';

/* 2.7 problem_handin表 */

DROP TABLE IF EXISTS `problem_handin`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`problem_handin`(
        `id` VARCHAR(36) NOT NULL COMMENT '题目提交id,uuid()',
        -- 题目提交id,uuid()
        `problem_id` VARCHAR(36) NOT NULL COMMENT '题目id',
        -- 题目id
        `student_id` VARCHAR(36) NOT NULL COMMENT '学生id',
        -- 学生id
        `code` TEXT NOT NULL COMMENT '学生提交代码',
        -- 学生提交代码
        `upload_time` DATETIME NOT NULL COMMENT '学生提交时间',
        -- 学生提交时间
        `language` VARCHAR(255) NOT NULL COMMENT '学生提交代码语言',
        -- 学生提交代码语言
        `examination_id` VARCHAR(36) COMMENT '学生提交所属测验id,若不属于测验则为null',
        -- 学生提交所属测验id,若不属于测验则为null
        `score` INT COMMENT '学生提交得分',
        -- 学生提交得分
        PRIMARY KEY (`id`),
        FOREIGN KEY (`problem_id`) REFERENCES `problem` (`id`),
        FOREIGN KEY (`student_id`) REFERENCES `user` (`id`),
        FOREIGN KEY (`examination_id`) REFERENCES `examination` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '题目提交表';

/* 2.8 examination_score表 */

DROP TABLE IF EXISTS `examination_score`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`examination_score`(
        `examination_id` VARCHAR(36) NOT NULL COMMENT '测验id',
        -- 测验id
        `student_id` VARCHAR(36) NOT NULL COMMENT '学生id',
        -- 学生id
        `score` INT NOT NULL COMMENT '学生测验成绩',
        -- 学生测验成绩
        `create_time` DATETIME NOT NULL COMMENT '学生测验成绩创建时间',
        -- 学生测验成绩创建时间
        `update_time` DATETIME NOT NULL COMMENT '学生测验成绩更新时间',
        -- 学生测验成绩更新时间
        PRIMARY KEY (
            `examination_id`,
            `student_id`
        ),
        FOREIGN KEY (`examination_id`) REFERENCES `examination` (`id`),
        FOREIGN KEY (`student_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '测验成绩表';

/* 2.9 experiment_score表 */

DROP TABLE IF EXISTS `experiment_score`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`experiment_score`(
        `id` VARCHAR(36) NOT NULL COMMENT '实验成绩id,uuid()',
        -- 实验成绩id,uuid()
        `experiment_id` VARCHAR(36) NOT NULL COMMENT '实验id',
        -- 实验id
        `student_id` VARCHAR(36) NOT NULL COMMENT '学生id',
        -- 学生id
        `score` INT NOT NULL COMMENT '学生实验成绩',
        -- 学生实验成绩
        `create_time` DATETIME NOT NULL COMMENT '学生实验成绩创建时间',
        -- 学生实验成绩创建时间
        `update_time` DATETIME NOT NULL COMMENT '学生实验成绩更新时间',
        -- 学生实验成绩更新时间
        PRIMARY KEY (`id`),
        FOREIGN KEY (`experiment_id`) REFERENCES `experiment` (`id`),
        FOREIGN KEY (`student_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '实验成绩表';

/* 2.10 examination_problem_list表 */

DROP TABLE IF EXISTS `examination_problem_list`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`examination_problem_list`(
        `examination_id` VARCHAR(36) NOT NULL COMMENT '测验id',
        -- 测验id
        `problem_id` VARCHAR(36) NOT NULL COMMENT '题目id',
        -- 题目id
        PRIMARY KEY (
            `examination_id`,
            `problem_id`
        ),
        FOREIGN KEY (`examination_id`) REFERENCES `examination` (`id`),
        FOREIGN KEY (`problem_id`) REFERENCES `problem` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '测验题目列表';

/* 2.11 examination_student_list表 */

DROP TABLE IF EXISTS `examination_student_list`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`examination_student_list`(
        `examination_id` VARCHAR(36) NOT NULL COMMENT '测验id',
        -- 测验id
        `student_id` VARCHAR(36) NOT NULL COMMENT '学生id',
        -- 学生id
        PRIMARY KEY (
            `examination_id`,
            `student_id`
        ),
        FOREIGN KEY (`examination_id`) REFERENCES `examination` (`id`),
        FOREIGN KEY (`student_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '测验学生列表';

/* 2.12 total_weight表 */

DROP TABLE IF EXISTS `total_weight`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`total_weight`(
        `id` VARCHAR(36) NOT NULL COMMENT '总权重id,uuid()',
        -- 总权重id,uuid()
        `experiment_weight` INT NOT NULL COMMENT '实验权重',
        -- 实验权重
        `examination_weight` INT NOT NULL COMMENT '测验权重',
        -- 测验权重
        `create_time` DATETIME NOT NULL COMMENT '总权重创建时间',
        -- 总权重创建时间
        `update_time` DATETIME NOT NULL COMMENT '总权重更新时间',
        -- 总权重更新时间
        PRIMARY KEY (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '总权重表';

/* 2.13 experiment_weight表 */

DROP TABLE IF EXISTS `experiment_weight`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`experiment_weight`(
        `id` VARCHAR(36) NOT NULL COMMENT '实验权重id,uuid()',
        -- 实验权重id,uuid()
        `experiment_id` VARCHAR(36) NOT NULL COMMENT '实验id',
        -- 实验id
        `weight` INT NOT NULL COMMENT '实验权重',
        -- 实验权重
        `create_time` DATETIME NOT NULL COMMENT '实验权重创建时间',
        -- 实验权重创建时间
        `update_time` DATETIME NOT NULL COMMENT '实验权重更新时间',
        -- 实验权重更新时间
        PRIMARY KEY (`id`),
        FOREIGN KEY (`experiment_id`) REFERENCES `experiment` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '实验权重表';

/* 2.14 examination_weight表 */

DROP TABLE IF EXISTS `examination_weight`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`examination_weight`(
        `id` VARCHAR(36) NOT NULL COMMENT '测验权重id,uuid()',
        -- 测验权重id,uuid()
        `examination_id` VARCHAR(36) NOT NULL COMMENT '测验id',
        -- 测验id
        `weight` INT NOT NULL COMMENT '测验权重',
        -- 测验权重
        `create_time` DATETIME NOT NULL COMMENT '测验权重创建时间',
        -- 测验权重创建时间
        `update_time` DATETIME NOT NULL COMMENT '测验权重更新时间',
        -- 测验权重更新时间
        PRIMARY KEY (`id`),
        FOREIGN KEY (`examination_id`) REFERENCES `examination` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '测验权重表';

/* 2.15 signin表 */

DROP TABLE IF EXISTS `signin`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`signin`(
        `id` VARCHAR(36) NOT NULL COMMENT '签到id,uuid()',
        -- 签到id,uuid()
        `teacher_id` VARCHAR(36) NOT NULL COMMENT '教师id',
        -- 教师id
        `name` VARCHAR(255) NOT NULL COMMENT '签到名称',
        -- 签到名称
        `start_time` DATETIME NOT NULL COMMENT '签到开始时间',
        -- 签到开始时间
        `end_time` DATETIME NOT NULL COMMENT '签到结束时间',
        -- 签到结束时间
        `create_time` DATETIME NOT NULL COMMENT '签到创建时间',
        -- 签到创建时间
        `update_time` DATETIME NOT NULL COMMENT '签到更新时间',
        -- 签到更新时间
        PRIMARY KEY (`id`),
        FOREIGN KEY (`teacher_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '签到表';

/* 2.16 signin_student表 */

DROP TABLE IF EXISTS `signin_student`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`signin_student`(
        `signin_id` VARCHAR(36) NOT NULL COMMENT '签到id',
        -- 签到id
        `student_id` VARCHAR(36) NOT NULL COMMENT '学生id',
        -- 学生id
        `create_time` DATETIME NOT NULL COMMENT '签到学生创建时间',
        -- 签到学生创建时间
        `update_time` DATETIME NOT NULL COMMENT '签到学生更新时间',
        -- 签到学生更新时间
        PRIMARY KEY (`signin_id`, `student_id`),
        FOREIGN KEY (`signin_id`) REFERENCES `signin` (`id`),
        FOREIGN KEY (`student_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '签到学生表';

/* 2.17 user_login表 */

DROP TABLE IF EXISTS `user_login`;

CREATE TABLE
    IF NOT EXISTS `SE2022`.`user_login`(
        `user_id` VARCHAR(36) NOT NULL COMMENT '用户id',
        -- 用户id
        `login_time` DATETIME NOT NULL COMMENT '用户登录时间',
        -- 用户登录时间
        `ip` VARCHAR(255) NOT NULL COMMENT '用户登录ip',
        -- 用户登录ip
        PRIMARY KEY (`user_id`, `login_time`),
        FOREIGN KEY (`user_id`) REFERENCES `user` (`id`)
    ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '用户登录表';