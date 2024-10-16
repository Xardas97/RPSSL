import { sendRequest, sendDeleteRequest } from "./baseApi";

export const apiGetScoreboard = async (take) => {
  return await sendRequest("play?take=" + take);
}

export const apiDeleteScoreboard = async () => {
  return await sendDeleteRequest("play");
}

export const apiDeleteGameRecord = async (id) => {
  return await sendDeleteRequest("play/" + id);
}