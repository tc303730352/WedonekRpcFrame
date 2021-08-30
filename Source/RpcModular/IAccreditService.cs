using RpcModular.Model;

namespace RpcModular
{
        public interface IAccreditService
        {
                IUserState CurrentUser { get; }
                string AddAccredit(string applyId, string[] roleId, UserState state);
                string ApplyTempAccredit(string[] role);
                void CancelAccredit(string accreditId);

                void KickOutAccredit(string applyId);
                void ToUpdate(string accreditId, UserState state, string[] roleId);

                IUserState GetAccredit(string accreditId);

                IAccreditService Create<T>() where T : UserState;

                void CheckAccredit(string accreditId);
        }
}