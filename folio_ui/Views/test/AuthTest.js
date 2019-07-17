/*
 * Authentication test
 * JS
*/

import Auth from "../Auth.js"
import assert from "assert";
import * as Cookies from "js-cookie";

// tests for Auth class
describe("Auth", () => {
    // tests if Auth.login() can perform login
    // NOTE: requires existing data in the database as defined in setup SQL
    describe(".login()", () => {
        const auth = new Auth(process.env.API_INGRESS);
        it("should perform login successfully and return true", async () => {
            const result = await auth.login("s1234112@ap.edu.sg", "p@55Student");
            assert.equal(result, true);
        });
    });

    // tests if Auth.check() can check login token
    // NOTE: requires existing data in the database as defined in setup SQL
    describe(".check()", () => {
        const auth = new Auth(process.env.API_INGRESS);
        auth.login("s1234112@ap.edu.sg", "p@55Student");

        it("should check auth token successfully", async () => {
            const result = await auth.check();
            assert.equal(result, true);
        });
    });
    
    // tests if Auth.info() can obtain user infomation
    // NOTE: requires existing data in the database as defined in setup SQL
    describe(".info()", () => {
        const auth = new Auth(process.env.API_INGRESS);
        auth.login("s1234112@ap.edu.sg", "p@55Student");

        it("should obtain user infomation of currently authenticated user", async () => {
            const userinfo = await auth.info();
            assert.equal(userinfo.id, 2);
            assert.equal(userinfo.name, "Amy Ng");
            assert.equal(userinfo.userRole, "Student");
            assert.equal(userinfo.emailAddr, "s1234112@ap.edu.sg");
        });
    });

    // tests if Auth.logout() can perform logout cleanly
    describe("logout()", () => {
        const auth = new Auth(process.env.API_INGRESS);
        auth.login("s1234112@ap.edu.sg", "p@55Student");

        it("should logout cleanly", async () => {
            auth.logout();
            assert.equal(auth.token, null);
            assert.equal(Cookies.get("Auth.token"), null);
        });
    });
});
