using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Entities;

public class LeaveType
{
    public int Id { get; private set; }
    public string Code { get; private set; } // new
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public int? DefaultDays { get; private set; } // new
    public bool IsPredefined { get; private set; } // new
    public LeaveRuleType Rule { get; private set; } // new

    public decimal PaidFraction { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    #region Domain_Navigation_Properties

    public IReadOnlyList<LeaveAllocation> LeaveAllocations { get; private set; }

    #endregion

    private LeaveType() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="LeaveType"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="leaveTypeRuleSet">Instance of <see cref="ILeaveTypeRuleSet"/> for rules checking</param>
    /// <param name="name">Name of leave type</param>
    /// <param name="description">Description for leave type</param>
    /// <param name="paidFraction">Paid fraction for multiplication by base salary</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="LeaveType"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<LeaveType> CreateAsync(ILeaveTypeRuleSet leaveTypeRuleSet,
                                                    string name,
                                                    decimal paidFraction = 1.0M,
                                                    string? description = null,
                                                    CancellationToken cancellationToken = default)
    {
        await ValidateBaseRulesAsync(leaveTypeRuleSet, name, paidFraction, cancellationToken);

        return new()
        {
            Name = name,
            Description = description,
            PaidFraction = paidFraction,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Updates existing <see cref="LeaveType"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="entity">Given <see cref="LeaveType"/> to be updated</param>
    /// <param name="leaveTypeRuleSet">Instance of <see cref="ILeaveTypeRuleSet"/> for rules checking</param>
    /// <param name="name">Name of leave type</param>
    /// <param name="description">Description for leave type</param>
    /// <param name="paidFraction">Paid fraction for multiplication by base salary</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public async Task UpdateAsync(ILeaveTypeRuleSet leaveTypeRuleSet,
                                  string name,
                                  decimal paidFraction = 1.0M,
                                  string? description = null,
                                  CancellationToken cancellationToken = default)
    {
        await ValidateBaseRulesAsync(leaveTypeRuleSet, name, paidFraction, cancellationToken);

        Name = name;
        Description = description;
        PaidFraction = paidFraction;
        ModifiedAt = DateTime.UtcNow;
    }

    private static async Task ValidateBaseRulesAsync(ILeaveTypeRuleSet leaveTypeRuleSet,
                                                     string name,
                                                     decimal paidFraction,
                                                     CancellationToken cancellationToken)
    {
        var isLeaveTypeNameUnique = await leaveTypeRuleSet.IsNameUniqueAsync(name, cancellationToken);

        if (!isLeaveTypeNameUnique)
        {
            throw new InvalidOperationException($"Leave type with name '{ name }' already exists");
        }

        if (paidFraction <= 0)
        {
            throw new ArgumentException("Paid fraction of leave type must be greater than 0");
        }
    }

    #endregion
}
