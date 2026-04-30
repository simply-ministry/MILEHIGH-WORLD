# Checkov Custom Policy for SOC 2 Encryption
# Verifies that EBS volumes are encrypted to meet CC6.6

# This file serves as a reference for Checkov scanning in CI/CD.
# It can also be implemented as a YAML/Python check if using the Checkov custom policy framework.

# Example Bridgecrew/Checkov YAML Policy (Internal Representation)
# metadata:
#   name: "Ensure EBS volumes are encrypted with KMS"
#   id: "CKV_AWS_SOC2_1"
#   category: "Encryption"
# scope:
#   provider: "aws"
#   resource: "aws_ebs_volume"
# check:
#   - encrypted:
#       operator: "equals"
#       value: true
#   - kms_key_id:
#       operator: "exists"

# In Terraform, we ensure compliance by setting the attributes correctly:
resource "aws_ebs_volume" "monitored_volume" {
  availability_zone = "us-east-1a"
  size              = 20
  encrypted         = true # MUST BE TRUE
  kms_key_id        = "arn:aws:kms:us-east-1:123456789012:key/some-key-id"
}
