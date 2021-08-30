using System;

namespace RpcModular.Model
{
        internal class UserCacheState : IUserState
        {
                private readonly IUserState _State = null;
                public UserCacheState(IAccreditService accredit)
                {
                        this._State = accredit.CurrentUser;
                }
                public object this[string name] { get => this._State[name]; set => this._State[name] = value; }

                public string AccreditId => this._State.AccreditId;

                public long SysGroupId => this._State.SysGroupId;

                public long RpcMerId => this._State.RpcMerId;

                public string[] Prower => this._State.Prower;



                public void Cancel()
                {
                        this._State.Cancel();
                }

                public bool CheckPrower(string prower)
                {
                        return this._State.CheckPrower(prower);
                }

                public T GetValue<T>(string name)
                {
                        return this._State.GetValue<T>(name);
                }

                public IUserState SaveState(Func<IUserState, IUserState, IUserState> upFun)
                {
                        return this._State.SaveState(upFun);
                }

                public void SaveState()
                {
                        this._State.SaveState();
                }
        }
}
