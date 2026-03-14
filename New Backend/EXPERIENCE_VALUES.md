# Experience Values Documentation

## Valid Experience Values

The `experienceGlobale` (call center experience) field accepts the following values:

| Value | Label (French) | Description |
|-------|---------------|-------------|
| `"0"` | Aucune expérience | No experience - Auto-assigned to Agent position |
| `"1"` | 0 - 6 mois | 0 to 6 months - Auto-assigned to Agent position |
| `"2"` | 6 mois - 12 mois | 6 months to 1 year - Auto-assigned to Agent position |
| `"3"` | 1 an - 2 ans | 1 to 2 years - Auto-assigned to Agent position |
| `"4"` | 2 ans - 3 ans | 2 to 3 years - Auto-assigned to Agent position |
| `"5"` | 3 ans - 5 ans | 3 to 5 years - Can choose position |
| `"6"` | 5 ans - 7 ans | 5 to 7 years - Can choose position |
| `"7"` | Plus de 7 ans | More than 7 years - Can choose position |

## Business Logic

### Auto-Assignment to Agent Position

When `experienceGlobale` is one of: `"0"`, `"1"`, `"2"`, `"3"`, `"4"`
- `posteRecherche` is automatically set to `"1"` (Agent)
- `experiencePosteRecherche` is not required and set to empty/null

### Position Selection Required

When `experienceGlobale` is one of: `"5"`, `"6"`, `"7"`
- User must select their desired position from the dropdown
- If they select a position other than Agent, they must specify their experience in that position

## Database Storage

All experience values are stored as strings in the `candidate_profiles` collection:
- Field: `call_center_experience`
- Type: String
- Values: "0", "1", "2", "3", "4", "5", "6", "7"

## Frontend Validation

The frontend validates that:
1. `experienceGlobale` is selected (required field)
2. If `experienceGlobale` is "5", "6", or "7" and user selects a non-Agent position, `experiencePosteRecherche` is required
3. All values are properly formatted before sending to backend

## Testing

To test "Aucune expérience" registration:
1. Select "Aucune expérience" from experience dropdown
2. Fill in all other required fields
3. Complete registration
4. Verify in MongoDB that `call_center_experience: "0"` is saved
5. Verify that `desired_position: "1"` (Agent) is auto-assigned
