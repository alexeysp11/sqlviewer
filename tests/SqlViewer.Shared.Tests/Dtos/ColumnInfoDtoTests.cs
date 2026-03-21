using System.Data;
using FluentAssertions;
using SqlViewer.Shared.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Shared.Tests.Dtos;

public sealed class ColumnInfoDtoTests
{
    #region Test cases
    public static TheoryData<TestCaseColumnInfoDto> GetTestCasesColumnInfoDto(VelocipedeDatabaseType databaseType) =>
    [
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, CharMaxLength = null, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, CharMaxLength = null, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, CharMaxLength = -1, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, CharMaxLength = -1, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, CharMaxLength = 0, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, CharMaxLength = 0, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, CharMaxLength = 10, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiString, CharMaxLength = 10, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Binary }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Binary, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Binary, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Byte }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Byte, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Byte, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Byte, NumericPrecision = null, NumericScale = null, DefaultValue = null, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Byte, NumericPrecision = null, NumericScale = null, DefaultValue = null, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Byte, NumericPrecision = 8, NumericScale = null, DefaultValue = 2, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Byte, NumericPrecision = 8, NumericScale = null, DefaultValue = 2, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean, DefaultValue = null, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean, DefaultValue = null, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean, DefaultValue = true, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean, DefaultValue = true, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean, DefaultValue = false, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Boolean, DefaultValue = false, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Currency }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Currency, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Currency, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Date }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Date, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Date, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTime }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTime, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTime, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Decimal }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Decimal, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Decimal, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Double }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Double, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Double, IsPrimaryKey = true }, },
        
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Guid }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Guid, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Guid, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int16 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int16, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int16, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int32 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int32, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int32, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int64 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int64, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Int64, IsPrimaryKey = true }, },
        
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Object }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Object, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Object, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.SByte }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.SByte, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.SByte, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.SByte, NumericPrecision = null, NumericScale = null, DefaultValue = null, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.SByte, NumericPrecision = null, NumericScale = null, DefaultValue = null, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.SByte, NumericPrecision = 8, NumericScale = null, DefaultValue = 2, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.SByte, NumericPrecision = 8, NumericScale = null, DefaultValue = 2, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Single }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Single, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Single, IsPrimaryKey = true }, },
        
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, CharMaxLength = null, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, CharMaxLength = null, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, CharMaxLength = -1, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, CharMaxLength = -1, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, CharMaxLength = 0, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, CharMaxLength = 0, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, CharMaxLength = 10, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.String, CharMaxLength = 10, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Time }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Time, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Time, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt16 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt16, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt16, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt32 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt32, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt32, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt64 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt64, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.UInt64, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.VarNumeric }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.VarNumeric, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.VarNumeric, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 0, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 0, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 10, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 10, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.StringFixedLength }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.StringFixedLength, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.StringFixedLength, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.StringFixedLength, CharMaxLength = null }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.StringFixedLength, CharMaxLength = -1 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.StringFixedLength, CharMaxLength = 0 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.StringFixedLength, CharMaxLength = 10 }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Xml }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Xml, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Xml, IsPrimaryKey = true }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Xml, CharMaxLength = null }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Xml, CharMaxLength = -1 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Xml, CharMaxLength = 0 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.Xml, CharMaxLength = 10 }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTime2 }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTime2, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTime2, IsPrimaryKey = true }, },

        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTimeOffset }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTimeOffset, IsPrimaryKey = false }, },
        new() { DatabaseType = databaseType, ColumnInfoDto = new() { ColumnName = "ColumnName", ColumnType = DbType.DateTimeOffset, IsPrimaryKey = true }, },
    ];
    #endregion  // Test cases

    [Theory]
    [MemberData(nameof(GetTestCasesColumnInfoDto), parameters: VelocipedeDatabaseType.None)]
    [MemberData(nameof(GetTestCasesColumnInfoDto), parameters: VelocipedeDatabaseType.InMemory)]
    [MemberData(nameof(GetTestCasesColumnInfoDto), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetTestCasesColumnInfoDto), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetTestCasesColumnInfoDto), parameters: VelocipedeDatabaseType.MSSQL)]
    public void ToVelocipedeColumnInfo(TestCaseColumnInfoDto testCase)
    {
        // Arrange.
        ColumnInfoDto columnInfoDto = testCase.ColumnInfoDto;
        VelocipedeDatabaseType databaseType = testCase.DatabaseType;

        // Act.
        VelocipedeColumnInfo result = columnInfoDto.ToVelocipedeColumnInfo(databaseType);

        // Assert.
        result
            .Should()
            .NotBeNull();
        result!.DatabaseType
            .Should()
            .Be(databaseType);
        result!.ColumnName
            .Should()
            .Be(columnInfoDto.ColumnName);
        result!.ColumnType
            .Should()
            .Be(columnInfoDto.ColumnType);
        result!.CharMaxLength
            .Should()
            .Be(columnInfoDto.CharMaxLength);
        result!.NumericPrecision
            .Should()
            .Be(columnInfoDto.NumericPrecision);
        result!.NumericScale
            .Should()
            .Be(columnInfoDto.NumericScale);
        result!.DefaultValue
            .Should()
            .Be(columnInfoDto.DefaultValue);
        result!.IsPrimaryKey
            .Should()
            .Be(columnInfoDto.IsPrimaryKey);
        result!.IsNullable
            .Should()
            .Be(columnInfoDto.IsNullable);
    }
}
