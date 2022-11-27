const axios = require('axios');
const https = require('https');
// import jest
const { expect } = require('@jest/globals');

const url = 'https://150.158.80.33:7191/api';
const agent = new axios.create({
    httpsAgent: new https.Agent({
        rejectUnauthorized: false
    })
});

// 登录测试
// id = "admin"
// password = "admin"
var token = ""; // 保存token

describe('登录测试', () => {
    it('管理员登录成功', async () => {
        const res = await agent.post(`${url}/login/account`, {
            id: 'admin',
            password: 'admin',
        });
        expect(res.status).toBe(200);
        expect(res.data.status).toBe('ok');
        expect(res.data.code).toBe(1000);
        expect(res.data.data.token).not.toBe(null);
        token = res.data.data.token;
    });
});

// 调用currentUser接口
// bearer token
describe('登录测试', () => {
    it('获取当前用户', async () => {
        const res = await agent.get(`${url}/currentUser`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        expect(res.status).toBe(200);
        expect(res.data.code).toBe(0);
        expect(res.data.status).toBe('ok');
        expect(res.data.data.user.id).toBe('admin');
    }
    )
});

// 访问AuthTest接口
// bearer token
describe('鉴权测试', () => {
    it('有效token访问AuthTest接口', async () => {
        const res = await agent.get(`${url}/AuthTest/test`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        expect(res.status).toBe(200);
    }
    )
});


// 无效token
describe('登录测试', () => {
    it('获取当前用户, 无效token', async () => {
        const res = await agent.get(`${url}/currentUser`, {
            headers: {
                Authorization: `Bearer ${token}123`
            }
        });
        expect(res.status).toBe(200);
        expect(res.data.status).toBe('error');
    }
    )
});

// 无效token访问AuthTest接口
// AuthTest/test
// 401
describe('鉴权测试', () => {
    it('无效token访问AuthTest接口', async () => {
        try {
            const res = await agent.get(`${url}/AuthTest/test`, {
                headers: {
                    Authorization: `Bearer ${token}123`
                }
            });
        } catch (e) {
            expect(e.response.status).toBe(401);
        }
    }
    )
});
