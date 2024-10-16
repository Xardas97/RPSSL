import { sendPostRequest } from "./baseApi.js";

export const apiLogin = async (username, passphrase) => {
  const requestBody = { name: username, passphrase: passphrase };
  return await sendPostRequest("login", requestBody, false)
}

export const apiRegister = async (username, passphrase) => {
  const requestBody = { name: username, passphrase: passphrase };
  return await sendPostRequest("register", requestBody, false)
}
