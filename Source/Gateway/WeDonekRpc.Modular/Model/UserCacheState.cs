using System;

namespace WeDonekRpc.Modular.Model
{
    internal class UserCacheState : IUserState
    {
        private readonly IUserState _State = null;
        public UserCacheState (IAccredit accredit)
        {
            this._State = accredit.CurrentUser;
        }
        public object this[string name] { get => this._State[name]; set => this._State[name] = value; }

        public string AccreditId => this._State.AccreditId;

        public string SysGroup => this._State.SysGroup;

        public long RpcMerId => this._State.RpcMerId;

        public string[] Prower => this._State.Prower;


        public void Cancel ()
        {
            this._State.Cancel();
        }
        public void SetPrower (string[] prowers)
        {
            this._State.SetPrower(prowers);
        }

        public bool CheckPrower (string[] prower)
        {
            return this._State.CheckPrower(prower);
        }
        public bool CheckPrower (string prower)
        {
            return this._State.CheckPrower(prower);
        }
        public T GetValue<T> (string name)
        {
            return this._State.GetValue<T>(name);
        }

        public IUserState SaveState (Func<IUserState, IUserState, IUserState> upFun)
        {
            return this._State.SaveState(upFun);
        }

        public bool SaveState ()
        {
            return this._State.SaveState();
        }

        public string ToJson ()
        {
            return this._State.ToJson();
        }
    }
}
