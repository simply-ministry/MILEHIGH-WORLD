# SOC 2 Hardening Module for AWS Resources
# Maps to CC7.2 (Threat Detection), CC4.1 (Monitoring), and P1.1 (Retention)

terraform {
  required_version = ">= 1.0.0"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}

module "security_automations_soc2" {
  source  = "squareops/security-automations/aws//modules/soc2"
  version = "1.0.1"

  enable_guard_duty         = true      # Threat detection (CC7.2)
  enable_security_hub       = true      # Compliance monitoring (CC4.1)
  region                    = var.aws_region
  s3_object_expiration_days = "90"      # Data retention (P1.1)
}

variable "aws_region" {
  description = "The AWS region to deploy hardening modules"
  type        = string
  default     = "us-east-1"
}

# Example of manual resource hardening to meet CC6.6 (Encryption)
resource "aws_kms_key" "storage_key" {
  description             = "KMS key for SOC 2 compliant storage encryption"
  deletion_window_in_days = 30
  enable_key_rotation     = true # Required for SOC 2 best practices
}

resource "aws_ebs_volume" "compliant_volume" {
  availability_zone = "${var.aws_region}a"
  size              = 40
  encrypted         = true  # Required for SOC 2 (CC6.6)
  kms_key_id        = aws_kms_key.storage_key.arn

  tags = {
    Compliance = "SOC2"
    Criterion  = "CC6.6"
  }
}
