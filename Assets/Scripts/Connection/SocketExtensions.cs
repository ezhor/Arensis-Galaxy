using System;
using BestHTTP.SocketIO3;
using UnityEngine;

namespace Connection
{
    public static class SocketExtensions
    {
        public static void OnTypedData<T>(this Socket socket, string eventName, Action<T> callback)
        {
            socket.On<string>(eventName, data =>
            {
                callback.Invoke(JsonUtility.FromJson<T>(data));
            });
        }
        
        public static void EmitTypedData(this Socket socket, string eventName, object data)
        {
            socket.Emit(eventName, JsonUtility.ToJson(data));
        }
    }
}