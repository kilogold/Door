using Brownie.Contracts.PointOfSale.ContractDefinition;
using Nethereum.Web3;

namespace dotnet
{
    public partial class Runtime
    {
        private class State_DoorCheck : BaseState
        {
            public static State_DoorCheck Instance { get; } = new();
            private State_DoorCheck(){}
            private Dictionary<Door.State, Tuple<string, IState.Option[]>> alternatives;
            public override string DisplayMessage => alternatives[Blackboard.Instance.door.CurrentState].Item1;
            public override IState.Option[] Options => alternatives[Blackboard.Instance.door.CurrentState].Item2;

            public override void Init()
            {
                alternatives = new()
                {
                    {
                        Door.State.Locked, new Tuple<string, IState.Option[]>(
                            "The door is locked.",
                            new[]
                            {
                                new IState.Option("Step back.", State_DoorInteraction.Instance),
                                new IState.Option("Pay to unlock.", State_PayAccess.Instance)
                            })
                    },
                
                    {
                        Door.State.Unlocked, new Tuple<string, IState.Option[]>(
                            "The door is unlocked. You traverse the door and it closes after you.\n" +
                            "You face the door once again.",
                            OptionTemplate_Continue(State_DoorInteraction.Instance)
                        )
                    },
                };
            }

            public override void RecursiveInit(HashSet<IState> initProgression)
            {
                if (initProgression.Contains(this))
                {
                    return;
                }
                
                Init();
                initProgression.Add(this);

                foreach (var alt in alternatives)
                {
                    foreach (var opt in alt.Value.Item2)
                    {
                        opt.StateInstance.RecursiveInit(initProgression);
                    }                    
                }
            }

            public override async Task Enter()
            {
                var web3 = Blackboard.Instance.web3;
                var func = new HasAccessFunction {
                    Addr = web3.TransactionManager.Account.Address // User's wallet.
                };
                
                var handler = web3.Eth.GetContractQueryHandler<HasAccessFunction>();
                var hasAccess = await handler.QueryAsync<bool>(Blackboard.Instance.posContractAddress, func);

                Console.WriteLine($"Contract access for {func.Addr}: {hasAccess}");
                Blackboard.Instance.door.CurrentState = hasAccess ? Door.State.Unlocked : Door.State.Locked;
            }
        }
    }
}